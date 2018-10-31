<template>
  <div class="app-container calendar-list-container">
    <div class="filter-container">
      <el-input @keyup.enter.native="handleFilter" style="width: 200px;" class="filter-item" placeholder="商户名称" v-model="listQuery.code">
      </el-input>
      <el-button class="filter-item" type="primary" icon="search" @click="handleFilter">搜索</el-button>
      <!-- <el-button class="filter-item" style="margin-left: 10px;" @click="handleCreate" type="primary" icon="edit">添加</el-button> -->
    </div>
    <el-table :data="list" v-loading.body="listLoading" element-loading-text="拼命加载中" border fit highlight-current-row>
      <el-table-column align="center" label='序号' width="95">
        <template scope="scope">
          {{scope.$index}}
        </template>
      </el-table-column>
      <el-table-column label="商户简称">
        <template scope="scope">
          <span class="link-type" @click="handleUpdate(scope.row)">{{scope.row.shortName}}</span>
        </template>
      </el-table-column>
      <el-table-column label="联系人" align="center">
        <template scope="scope">
          <span>{{scope.row.contactMan}}</span>
        </template>
      </el-table-column>
      <!-- <el-table-column label="商户城市"  align="center">
        <template scope="scope">
          <span>{{scope.row.city}}</span>
        </template>
      </el-table-column> -->
      <el-table-column label="联系手机号" align="center">
        <template scope="scope">
          <span>{{scope.row.mobilePhone}}</span>
        </template>
      </el-table-column>

      <el-table-column label="操作" width="80">
        <template scope="scope">
          <el-button @click.prevent="handleUpdate(scope.row)" type="text" size="small">编辑</el-button>
        </template>
      </el-table-column>
    </el-table>

    <div v-show="!listLoading" class="pagination-container">
      <el-pagination @size-change="handleSizeChange" @current-change="handleCurrentChange" :current-page.sync="listQuery.page" :page-sizes="[10,20,30, 50]" :page-size="listQuery.pagesize" layout="total, sizes, prev, pager, next, jumper" :total="total">
      </el-pagination>
    </div>

    <el-dialog :title="textMap[dialogStatus]" :visible.sync="dialogFormVisible" :close-on-click-modal="false">
      <el-form class="small-space" :model="ruleForm" :rules="rules" ref="ruleForm" label-position="left" label-width="150px" style='width: 620px; margin-left:50px;'>
        <el-form-item label="密码" prop="password">
          <el-input v-model="ruleForm.password" type="password"></el-input>
        </el-form-item>
      </el-form>

      <div slot="footer" class="dialog-footer">
        <el-button @click="dialogFormVisible = false">取 消</el-button>
        <el-button v-if="dialogStatus=='create'" type="primary" @click="create('ruleForm')">确 定 </el-button>
        <el-button v-else type="primary" @click="update('ruleForm')">确 定</el-button>
      </div>
    </el-dialog>
  </div>
</template>

<script>
import {
  GetByBusiness,
  editfamerchant,
  getmerchantcard
} from '@/api/merchant.js'
import { merchantcity, businesscategory, bankcode } from '@/api/basic.js'
import { payTestByOther } from '@/api/java_order.js'
import md5 from 'js-md5'
import { mapGetters } from 'vuex'

export default {
  data() {
    var checkamount = (rule, value, callback) => {
      if (!Number.parseFloat(value) || value <= 0) {
        callback(new Error('请输入有效数字值'))
      } else {
        var reg = /^\d+(\.([1-9]|\d[1-9]))?$/
        if (!reg.test(value)) {
          callback(new Error('收款金额最多保留两位小数'))
        } else {
          callback()
        }
      }
    }
    var checkcard_bankaccountNo = (rule, value, callback) => {
      if (!this.card_bankaccountNo) {
        callback(new Error('请选择银行卡'))
      } else {
        callback()
      }
    }
    return {
      list: null,
      total: null,
      listLoading: true,
      listQuery: {
        page: 1,
        pagesize: 20,
        code: '',
        merchantid: 0
      },
      citylist: [], //城市列表
      business_category: [], //经营类目
      bank_code: [], //银行代码
      ruleForm: '',
      dialogFormVisible: false,
      dialogStatus: '',
      textMap: {
        update: '编辑',
        create: '创建'
      },
      card_bankaccountNo: '', //开户账号
      bankaccountNoList: [],
      dialogPvVisible: false,
      rules: {
        password: [{ required: true, message: '请输入密码', trigger: 'blur' }]
      }
    }
  },
  computed: {
    ...mapGetters(['appid', 'appsecret'])
  },
  filters: {
    statusFilter(status) {
      const statusMap = {
        1: '启用',
        0: '关闭'
      }
      return statusMap[status]
    },
    dateFilter(value) {
      if (value) {
        return value.replace('T', ' ')
      }
    }
  },
  created() {
    this.fetchData()
    this.getbasic()
  },
  methods: {
    resetTemp() {
      this.ruleForm = {
        amount: '', //提现金额
        appid: '', //appid
        card_bankaccountNo: '' //开户账号
      }
    },
    handleFilter() {
      this.listQuery.page = 1
      this.fetchData()
    },
    handleCurrentChange(val) {
      this.listQuery.page = val
      this.fetchData()
    },
    handleSizeChange(val) {
      this.listQuery.pagesize = val
      this.getList()
    },
    handleCreate() {
      this.resetTemp()
      this.dialogStatus = 'create'
      this.dialogFormVisible = true
    },
    handleUpdate(row) {
      this.ruleForm = Object.assign({}, row)
      this.ruleForm.password = null
      this.dialogStatus = 'update'
      this.dialogFormVisible = true
    },
    // handleDelete(row) {
    //   this.$confirm("此操作删除商家, 是否继续?", "提示", {
    //     confirmButtonText: "确定",
    //     cancelButtonText: "取消",
    //     type: "warning"
    //   }).then(() => {
    //     this.ruleForm = Object.assign({}, row);
    //     businessbasic("delete", { id: this.ruleForm.id }).then(response => {
    //       this.dialogFormVisible = false;
    //       this.fetchData();
    //       this.$notify({
    //         title: "成功",
    //         message: "删除成功",
    //         type: "success",
    //         duration: 2000
    //       });
    //     });
    //   });
    // },
    fetchData() {
      this.listLoading = true
      GetByBusiness('get').then(response => {
        this.list = response.data
        this.total = response.total

        this.listLoading = false
      })
      // console.log(bData);
      // this.list = bData.router;
      // this.total = bData.router.length;
      // this.listLoading = false;
    },
    getbasic() {
      merchantcity('get').then(response => {
        this.citylist = response.data
      })
      businesscategory('get').then(response => {
        this.business_category = response.data
      })
      bankcode('get').then(response => {
        this.bank_code = response.data
      })
      getmerchantcard('get').then(response => {
        this.bankaccountNoList = response.data
      })
    },
    update(formName) {
      this.$refs[formName].validate(valid => {
        if (valid) {
          editfamerchant('put', this.ruleForm).then(response => {
            this.dialogFormVisible = false
            this.fetchData()
            var _this = this
            this.$notify({
              title: '成功',
              message: '修改成功',
              type: 'success',
              duration: 800,
              onClose: function() {
                _this.$store.dispatch('LogOut').then(() => {
                  location.reload() // 为了重新实例化vue-router对象 避免bug
                })
              }
            })
          })
        } else {
          console.log('error submit!!')
          return false
        }
      })
    }
  },
  watch: {
    dialogFormVisible(val, oldValue) {
      if (!val) {
        this.resetTemp()
        this.$refs['ruleForm'].resetFields()
      }
    }
  }
}
</script>
<style scoped>
.filter-container {
  margin-bottom: 20px;
}
.link-type:hover,
.link-type:focus:hover {
  color: #20a0ff;
}
.link-type,
.link-type:focus {
  color: #337ab7;
  cursor: pointer;
}
.pagination-container {
  margin-top: 20px;
}
.el-tag + .el-tag {
  margin-left: 10px;
}
</style>
