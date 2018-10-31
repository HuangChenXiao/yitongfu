<template>
  <div class="app-container calendar-list-container">
    <div class="filter-container">
      <el-input @keyup.enter.native="handleFilter" style="width: 200px;" class="filter-item" placeholder="用户编号/名称" v-model="listQuery.code">
      </el-input>
      <el-select v-model="listQuery.status" filterable placeholder="状态" class="filter-item">
        <el-option :value="2" label="全部">
        </el-option>
        <el-option :value="0" label="禁用">
        </el-option>
        <el-option :value="1" label="启用">
        </el-option>
      </el-select>
      <el-button class="filter-item" type="primary" icon="search" @click="handleFilter">搜索</el-button>
      <el-button class="filter-item" style="margin-left: 10px;" @click="handleCreate" type="primary" icon="edit">添加</el-button>
    </div>
    <el-table :data="list" v-loading.body="listLoading" element-loading-text="拼命加载中" border fit highlight-current-row>
      <el-table-column align="center" label='序号' width="95">
        <template scope="scope">
          {{scope.$index+1}}
        </template>
      </el-table-column>
      <el-table-column label="sh_appid" align="center" width="200">
        <template scope="scope">
          <span class="link-type" @click="handleUpdate(scope.row)">{{scope.row.sh_appid}}</span>
        </template>
      </el-table-column>
      <el-table-column label="sh_appsecret" align="center" width="200">
        <template scope="scope">
          <span>{{scope.row.sh_appsecret}}</span>
        </template>
      </el-table-column>
      <el-table-column label="门店ID" align="center">
        <template scope="scope">
          <span>{{scope.row.sh_storeid}}</span>
        </template>
      </el-table-column>
      <el-table-column align="center" prop="created_at" label="门店名称" width="100">
        <template scope="scope">
          <span>{{scope.row.sh_storename}}</span>
        </template>
      </el-table-column>
      <el-table-column align="center" prop="created_at" label="商户ID">
        <template scope="scope">
          <span>{{scope.row.sh_mobile}}</span>
        </template>
      </el-table-column>
      <el-table-column align="center" prop="created_at" label="商户名称" width="200">
        <template scope="scope">
          <span>{{scope.row.sh_businessname}}</span>
        </template>
      </el-table-column>

      <el-table-column align="center" prop="created_at" label="保证金">
        <template scope="scope">
          <span>{{scope.row.sh_balance}}</span>
        </template>
      </el-table-column>
      <el-table-column align="center" prop="created_at" label="佣金">
        <template scope="scope">
          <span>{{scope.row.sh_commission}}</span>
        </template>
      </el-table-column>
      <el-table-column align="center" prop="created_at" label="佣金比例%">
        <template scope="scope">
          <span>{{scope.row.sh_commratio}}</span>
        </template>
      </el-table-column>

      <el-table-column align="center" prop="created_at" label="代理商名称" width="200">
        <template scope="scope">
          <span>{{scope.row.agentname}}</span>
        </template>
      </el-table-column>

      <el-table-column align="center" prop="created_at" label="状态" width="100">
        <template scope="scope">
          <el-tag :type="scope.row.status==true?'success':''">{{scope.row.status==true?"启用":"禁用"}}</el-tag>
          </span>
        </template>
      </el-table-column>
      <el-table-column label="操作" width="180">
        <template scope="scope">
          <el-button @click.prevent="handleUpdate(scope.row)" type="text" size="small">编辑</el-button>
          <el-button @click.prevent="handleChongZhi(scope.row)" type="text" size="small">充值</el-button>
          <el-button @click.prevent="quicklogon(scope.row)" type="text" size="small">一键登录</el-button>
        </template>
      </el-table-column>
    </el-table>

    <div v-show="!listLoading" class="pagination-container">
      <el-pagination @size-change="handleSizeChange" @current-change="handleCurrentChange" :current-page.sync="listQuery.page" :page-sizes="[10,20,30, 50]" :page-size="listQuery.pagesize" layout="total, sizes, prev, pager, next, jumper" :total="total">
      </el-pagination>
    </div>

    <el-dialog :title="textMap[dialogStatus]" :visible.sync="dialogFormVisible">
      <el-form class="small-space" :model="ruleForm" :rules="rules" ref="ruleForm" label-position="right" label-width="100px" style='width: 400px; margin-left:50px;'>
        <el-form-item label="代理商名称" prop="agid">
          <el-select v-model="ruleForm.agid" placeholder="请选择代理商" style="width:100%">
            <el-option v-for="item in agentlist" :key="item.id" :label="item.name" :value="item.id">
            </el-option>
          </el-select>
        </el-form-item>
        <el-form-item label="sh_appid" prop="sh_appid">
          <el-input v-model="ruleForm.sh_appid"></el-input>
        </el-form-item>
        <el-form-item label="sh_appsecret" prop="sh_appsecret">
          <el-input v-model="ruleForm.sh_appsecret"></el-input>
        </el-form-item>
        <el-form-item label="商户ID" prop="sh_mobile">
          <el-input v-model="ruleForm.sh_mobile"></el-input>
        </el-form-item>
        <el-form-item label="密码" :prop="dialogStatus=='create'?'sh_password':''">
          <el-input v-model="ruleForm.sh_password" type="password"></el-input>
        </el-form-item>
        <el-form-item label="门店ID" prop="sh_storeid">
          <el-input v-model="ruleForm.sh_storeid"></el-input>
        </el-form-item>
        <el-form-item label="门店名称" prop="sh_storename">
          <el-input v-model="ruleForm.sh_storename"></el-input>
        </el-form-item>
        <el-form-item label="商户名称" prop="sh_businessname">
          <el-input v-model="ruleForm.sh_businessname"></el-input>
        </el-form-item>
        <el-form-item label="佣金比例%" prop="sh_commratio">
          <el-input v-model="ruleForm.sh_commratio"></el-input>
        </el-form-item>
        <el-form-item label="状态" prop="status">
          <el-radio v-model="ruleForm.status" :label="false">禁用</el-radio>
          <el-radio v-model="ruleForm.status" :label="true">启用</el-radio>
        </el-form-item>
      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button @click="dialogFormVisible = false">取 消</el-button>
        <el-button v-if="dialogStatus=='create'" type="primary" @click="create('ruleForm')">确 定</el-button>
        <el-button v-else type="primary" @click="update('ruleForm')">确 定</el-button>
      </div>
    </el-dialog>
    <el-dialog title="充值" :visible.sync="dialogChongZhiFormVisible">
      <el-form class="small-space" :model="ruleForm" :rules="rules" ref="ruleForm" label-position="right" label-width="100px" style='width: 400px; margin-left:50px;'>
        <el-form-item label="金额" prop="amount">
          <el-input v-model="ruleForm.amount"></el-input>
        </el-form-item>
      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button @click="dialogChongZhiFormVisible = false">取 消</el-button>
        <el-button @click="chongzhi('ruleForm')">确 定</el-button>
      </div>
    </el-dialog>
  </div>
</template>

<script>
import {
  agent,
  SHBusinessAppinfoList,
  EditSHBusinessAppinfoList
} from '@/api/user.js'
import { validatenumber } from '@/utils/validate.js'
import md5 from 'js-md5'
export default {
  data() {
    var checkAgId = (rule, value, callback) => {
      //代理商
      if (!value) {
        return callback(new Error('请选择代理商'))
      } else {
        callback()
      }
    }
    var number_change = (rule, value, callback) => {
      if (!validatenumber(value)) {
        callback(new Error('请输入数值类型'))
      } else {
        callback()
      }
    }
    return {
      list: null,
      total: null,
      selectlist: null,
      listLoading: true,
      agentlist: [],
      listQuery: {
        page: 1,
        pagesize: this.basepagesize,
        code: '',
        status: 2
      },
      ruleForm: {
        userid: '',
        roleid: null,
        usercode: '',
        username: '',
        password: ''
      },
      dialogFormVisible: false,
      dialogChongZhiFormVisible: false,
      dialogStatus: '',
      textMap: {
        update: '编辑',
        create: '创建'
      },
      dialogPvVisible: false,
      pvData: [],
      rules: {
        sh_appid: [
          { required: true, message: '请输入sh_appid', trigger: 'blur' }
        ],
        sh_appsecret: [
          { required: true, message: '请输入sh_appsecret', trigger: 'blur' }
        ],
        sh_storeid: [
          { required: true, message: '请输入门店ID', trigger: 'blur' }
        ],
        sh_storename: [
          { required: true, message: '请输入门店名称', trigger: 'blur' }
        ],
        sh_businessname: [
          { required: true, message: '请输入商户名称', trigger: 'blur' }
        ],
        sh_commratio: [
          { required: true, trigger: 'blur', validator: number_change }
        ],
        sh_mobile: [
          { required: true, message: '请输入手机号', trigger: 'blur' }
        ],
        sh_password: [
          { required: true, message: '请输入密码', trigger: 'blur' }
        ],
        agid: [{ validator: checkAgId, trigger: 'change' }],
        sh_commratio: [
          { required: true, trigger: 'blur', validator: number_change }
        ]
      }
    }
  },
  filters: {
    statusFilter(status) {
      const statusMap = {
        1: '启用',
        0: '禁用'
      }
      return statusMap[status]
    }
  },
  created() {
    this.fetchData()
    agent('get', { page: 1, pagesize: 9999, code: '' }).then(response => {
      this.agentlist = response.data
    })
  },
  methods: {
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
      this.ruleForm.sh_password = ''
      this.dialogStatus = 'update'
      this.dialogFormVisible = true
    },
    handleChongZhi(row) {
      this.ruleForm = Object.assign({}, row)
      this.dialogChongZhiFormVisible = true
    },
    quicklogon(row) {
      // if (row.bus_domain) {
      //   window.open(
      //     'http://' +
      //       row.bus_domain +
      //       '/#/quicklogon/' +
      //       row.merchantName +
      //       '/' +
      //       row.password +
      //       '/' +
      //       md5(row.merchantName + row.password + 'buyunchina')
      //   )
      // } else {
      //   window.open(
      //     'http://xinsybusiness.chaomafu.com/#/quicklogon/' +
      //       row.merchantName +
      //       '/' +
      //       row.password +
      //       '/' +
      //       md5(row.merchantName + row.password + 'buyunchina')
      //   )
      // }
      // debugger
      window.open("http://xinsysh.chaomafu.com/#/quicklogon/"+row.sh_mobile+"/"+row.sh_password+"/"+md5(row.sh_mobile+row.sh_password+"buyunchina"))
    },
    resetTemp() {
      this.ruleForm = {
        agentname: null,
        agid: null,
        count: null,
        id: null,
        sh_appid: '',
        sh_appsecret: '',
        sh_balance: null,
        sh_businessname: null,
        sh_commission: null,
        sh_commratio: null,
        sh_mobile: null,
        sh_storeid: '',
        sh_storename: '',
        status: true,
        amount: null
      }
    },
    fetchData() {
      this.listLoading = true
      SHBusinessAppinfoList('get', this.listQuery).then(response => {
        this.list = response.data
        this.total = response.data ? response.data[0].count : 0

        this.listLoading = false
      })
    },
    update(formName) {
      this.$refs[formName].validate(valid => {
        if (valid) {
          EditSHBusinessAppinfoList('put', this.ruleForm).then(response => {
            this.dialogFormVisible = false
            this.fetchData()
            this.$notify({
              title: '成功',
              message: '更新成功',
              type: 'success',
              duration: 2000
            })
          })
        } else {
          console.log('error submit!!')
          return false
        }
      })
    },
    chongzhi(formName) {
      this.$refs[formName].validate(valid => {
        if (valid) {
          this.$confirm('该操作充值保证金, 是否继续?', '提示', {
            confirmButtonText: '确定',
            cancelButtonText: '取消',
            type: 'warning'
          }).then(() => {
            var data = {}
            data.sh_businessid = this.ruleForm.id
            data.amount = this.ruleForm.amount
            SHBusinessAppinfoList('get', data).then(response => {
              this.dialogChongZhiFormVisible = false
              this.fetchData()
              this.$notify({
                title: '成功',
                message: '更新成功',
                type: 'success',
                duration: 2000
              })
            })
          })
        } else {
          console.log('error submit!!')
          return false
        }
      })
    },
    create(formName) {
      this.$refs[formName].validate(valid => {
        if (valid) {
          EditSHBusinessAppinfoList('post', this.ruleForm).then(response => {
            this.dialogFormVisible = false
            this.fetchData()
            this.$notify({
              title: '成功',
              message: '新增成功',
              type: 'success',
              duration: 2000
            })
          })
        } else {
          console.log('error submit!!')
          return false
        }
      })
    },
    handleSelectionChange(val) {
      this.multipleSelection = val
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
</style>
