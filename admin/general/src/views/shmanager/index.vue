<template>
    <div class="app-container calendar-list-container">
        <div class="filter-container">
            <!-- <el-input @keyup.enter.native="handleFilter" style="width: 200px;" class="filter-item" placeholder="子商户ID" v-model="listQuery.sh_businessid">
            </el-input> -->
            <el-select v-model="listQuery.status" filterable placeholder="状态" class="filter-item">
                <el-option :value="3" label="全部">
                </el-option>
                <el-option :value="0" label="未支付">
                </el-option>
                <el-option :value="1" label="审核成功">
                </el-option>
                <el-option :value="2" label="审核失败">
                </el-option>
            </el-select>
            <el-date-picker v-model="listQuery.begindate" type="date" placeholder="开始日期">
            </el-date-picker>
            <el-date-picker v-model="listQuery.enddate" type="date" placeholder="结束日期">
            </el-date-picker>

            <el-button class="filter-item" type="primary" icon="search" @click="handleFilter">搜索</el-button>
            <!-- <el-button class="filter-item" style="margin-left: 10px;" @click="handleCreate" type="primary" icon="edit">添加</el-button> -->
        </div>
        <el-table :data="list" v-loading.body="listLoading" element-loading-text="拼命加载中" border fit highlight-current-row>
            <el-table-column align="center" label='序号' width="95">
                <template scope="scope">
                    {{scope.$index+1}}
                </template>
            </el-table-column>
            <el-table-column label="订单号" align="center">
                <template scope="scope">
                    <span>{{scope.row.commorderno}}</span>
                </template>
            </el-table-column>
            <el-table-column label="银行卡号" align="center">
                <template scope="scope">
                    <span>{{scope.row.bankaccountno}}</span>
                </template>
            </el-table-column>
            <el-table-column label="银行名称" align="center">
                <template scope="scope">
                    <span>{{scope.row.bankname}}</span>
                </template>
            </el-table-column>
            <el-table-column label="银行地址" align="center">
                <template scope="scope">
                    <span>{{scope.row.bankaddress}}</span>
                </template>
            </el-table-column>
            <el-table-column label="姓名" align="center">
                <template scope="scope">
                    <span>{{scope.row.bankusername}}</span>
                </template>
            </el-table-column>
            <el-table-column label="手机号" align="center">
                <template scope="scope">
                    <span>{{scope.row.mobileno}}</span>
                </template>
            </el-table-column>
            <el-table-column label="商户ID" align="center">
                <template scope="scope">
                    <span>{{scope.row.sh_businessid}}</span>
                </template>
            </el-table-column>
            <el-table-column label="商户名称" align="center">
                <template scope="scope">
                    <span>{{scope.row.sh_businessname}}</span>
                </template>
            </el-table-column>
            <el-table-column label="门店名称" align="center">
                <template scope="scope">
                    <span>{{scope.row.sh_storename}}</span>
                </template>
            </el-table-column>

            <el-table-column label="原因" align="center">
                <template scope="scope">
                    <span>{{scope.row.reason}}</span>
                </template>
            </el-table-column>

            <el-table-column label="状态" align="center">
                <template scope="scope">
                    <span>
                        <el-tag :type="scope.row.status==1?'success':''">{{scope.row.status|statusFilter}}</el-tag>
                    </span>
                </template>
            </el-table-column>
            <el-table-column align="center" prop="created_at" label="新增时间" width="200">
                <template scope="scope">
                    <i class="el-icon-time"></i>
                    <span>{{scope.row.addtime}}</span>
                </template>
            </el-table-column>
            <el-table-column label="操作" width="80">
                <template scope="scope">
                    <el-button @click.prevent="handleUpdate(scope.row)" :disabled="scope.row.status!=0" type="text" size="small">审核</el-button>
                </template>
            </el-table-column>
        </el-table>

        <div v-show="!listLoading" class="pagination-container">
            <el-pagination @size-change="handleSizeChange" @current-change="handleCurrentChange" :current-page.sync="listQuery.page" :page-sizes="[10,20,30, 50]" :page-size="listQuery.pagesize" layout="total, sizes, prev, pager, next, jumper" :total="total">
            </el-pagination>
        </div>

        <el-dialog :title="textMap[dialogStatus]" :visible.sync="dialogFormVisible">
            <el-form class="small-space" :model="ruleForm" :rules="rules" ref="ruleForm" label-position="right" label-width="100px" style='width: 400px; margin-left:50px;'>
                <el-form-item label="状态" prop="status">
                    <el-radio v-model="ruleForm.status" :label="1">审核通过</el-radio>
                    <el-radio v-model="ruleForm.status" :label="2">审核失败</el-radio>
                </el-form-item>
                <el-form-item label="原因" prop="reason">
                    <el-input v-model="ruleForm.reason"></el-input>
                </el-form-item>
            </el-form>
            <div slot="footer" class="dialog-footer">
                <el-button @click="dialogFormVisible = false">取 消</el-button>
                <el-button @click="update('ruleForm')" type="primary">确 定</el-button>
            </div>
        </el-dialog>
    </div>
</template>

<script>
import { BusinessCommList } from '@/api/shmanager.js'
import { merchant } from '@/api/user.js'
import { validatenumber } from '@/utils/validate.js'
export default {
  data() {
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
      listQuery: {
        sh_businessid: null,
        status: 3,
        begindate: this.cfg.getCurrentMonthFirst(),
        enddate: this.cfg.getCurrentMonthLast(),
        page: 1,
        pagesize: this.basepagesize
      },
      ruleForm: {},
      mer_list: [],
      dialogFormVisible: false,
      dialogStatus: '',
      textMap: {
        update: '编辑',
        create: '创建'
      },
      dialogPvVisible: false,
      pvData: [],
      rules: {
        reason: [{ required: true, message: '请输入原因', trigger: 'blur' }]
      }
    }
  },
  filters: {
    statusFilter(status) {
      const statusMap = {
        0: '未审核',
        1: '审核成功',
        2: '审核失败'
      }
      return statusMap[status]
    }
  },
  created() {
    this.fetchData()
    merchant('get', {
      page: 1,
      pagesize: 9999
    }).then(response => {
      this.mer_list = response.data
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
      this.ruleForm.status = 1
      this.ruleForm.password = ''
      this.dialogStatus = 'update'
      this.dialogFormVisible = true
    },
    resetTemp() {
      this.ruleForm = {
        merchantid: '',
        comm_amount: '',
        memo: '',
        type: '1'
      }
    },
    fetchData() {
      this.listLoading = true
      BusinessCommList('get', this.listQuery).then(response => {
        this.list = response.data
        this.total = response.data ? response.data[0].count : 0

        this.listLoading = false
      })
    },
    update(formName) {
      this.$refs[formName].validate(valid => {
        if (valid) {
          BusinessCommList('get', this.ruleForm).then(response => {
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
    create(formName) {
      this.$refs[formName].validate(valid => {
        if (valid) {
          EditMercomm('get', this.ruleForm).then(response => {
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
