<template>
    <div class="app-container calendar-list-container">
        <div class="filter-container" style="position: relative;">
            <el-input @keyup.enter.native="handleFilter" style="width: 200px;" v-model="listQuery.keyword" class="filter-item" placeholder="订单号/备注">
            </el-input>
            <el-select class="filter-item" style="width: 130px" v-model="listQuery.status" placeholder="类型">
                <el-option v-for="item in  statusList" :key="item.key" :label="item.name" :value="item.key">
                </el-option>
            </el-select>
            <el-select class="filter-item" style="width: 130px" v-model="listQuery.paytype" placeholder="支付类型">
                <el-option key="1" label="微信" value="1">
                </el-option>
                <el-option key="2" label="支付宝" value="2">
                </el-option>
            </el-select>
            <el-date-picker v-model="listQuery.begindate" type="date" placeholder="开始日期">
            </el-date-picker>
            -
            <el-date-picker v-model="listQuery.enddate" type="date" placeholder="结束日期">
            </el-date-picker>

            <el-button class="filter-item" type="primary" icon="search" @click="handleFilter">搜索</el-button>
            <!-- <el-button class="filter-item" type="primary" @click="handleBaoZ(2)">保证金充值</el-button> -->
            <!-- <el-button class="filter-item" type="primary" @click="handleExcel">导出</el-button> -->

        </div>
        <el-table :data="list" v-loading.body="listLoading" element-loading-text="拼命加载中" border fit highlight-current-row @selection-change="handleSelectionChange">
            <el-table-column type="selection" width="55">
            </el-table-column>
            <el-table-column align="center" label='序号' width="95">
                <template scope="scope">
                    {{scope.$index +1}}
                </template>
            </el-table-column>
            <el-table-column label="二维码">
                <template scope="scope">
                    <a :href="scope.row.qrcode">
                        <img :src="scope.row.qrcode" width="40" alt="">
          </a>
                </template>
            </el-table-column>
            <el-table-column label="二维码名称">
                <template scope="scope">
                    <span>{{scope.row.qrname}}</span>
                </template>
            </el-table-column>
            <el-table-column label="商户名称">
                <template scope="scope">
                    <span>{{scope.row.sh_businessname}}</span>
                </template>
            </el-table-column>
            <el-table-column label="门店名称" align="center">
                <template scope="scope">
                    <span>{{scope.row.sh_storename}}</span>
                </template>
            </el-table-column>
            <el-table-column label="手机号" align="center">
                <template scope="scope">
                    <span>{{scope.row.sh_mobile}}</span>
                </template>
            </el-table-column>
            <el-table-column label="金额" align="center">
                <template scope="scope">
                    <span>{{scope.row.amount}}</span>
                </template>
            </el-table-column>
            <el-table-column label="订单状态" align="center">
                <template scope="scope">
                    <el-tag :type="scope.row.status==1?'success':''">{{scope.row.status| statusFilter}}</el-tag>
                </template>
            </el-table-column>
            <el-table-column label="订单状态" align="center">
                <template scope="scope">
                    <el-tag :type="'success'">{{scope.row.paytype==1?'微信':'支付宝'}}</el-tag>
                </template>
            </el-table-column>
            <el-table-column label="备注" align="center">
                <template scope="scope">
                    <span>{{scope.row.remark}}</span>
                </template>
            </el-table-column>
            <el-table-column label="充值时间" align="center">
                <template scope="scope">
                    {{scope.row.addtime}}
                </template>
            </el-table-column>
            <el-table-column label="操作" width="80">
                <template scope="scope">
                    <el-button @click.prevent="handleUpdate(scope.row,1)" :disabled="scope.row.status!=0" type="text" size="small">审核</el-button>
                </template>
            </el-table-column>
        </el-table>
        <div v-show="!listLoading" class="pagination-container">
            <el-pagination @size-change="handleSizeChange" @current-change="handleCurrentChange" :current-page.sync="listQuery.page" :page-sizes="[10,20,30, 50]" :page-size="listQuery.pagesize" layout="total, sizes, prev, pager, next, jumper" :total="total">
            </el-pagination>
        </div>
        <el-dialog :title="textMap[dialogStatus]" :visible.sync="dialogFormVisible">
            <el-form class="small-space" :model="ruleForm" :rules="rules" ref="ruleForm" label-position="right" label-width="100px" style='width: 400px; margin-left:50px;'>
                <div v-if="update_type==1">
                    <el-form-item label="状态" prop="status">
                        <el-radio label="1" v-model="ruleForm.status">已处理</el-radio>
                        <el-radio label="2" v-model="ruleForm.status">退回</el-radio>
                    </el-form-item>
                    <el-form-item label="原因" prop="reason">
                        <el-input v-model="ruleForm.reason"></el-input>
                    </el-form-item>
                </div>
                <div v-if="update_type==2">
                    <el-form-item label="商户" prop="sh_businessid">
                        <el-select filterable v-model="ruleForm.sh_businessid" placeholder="请选择商户" style="width:100%">
                            <el-option v-for="item in sh_business_list" :key="item.id" :label="item.sh_businessname" :value="item.id">
                            </el-option>
                        </el-select>
                    </el-form-item>
                    <el-form-item label="金额" prop="amount">
                        <el-input v-model="ruleForm.amount"></el-input>
                    </el-form-item>
                    <el-form-item label="状态" prop="paytype">
                        <el-radio :label="1" v-model="ruleForm.paytype">微信</el-radio>
                        <el-radio :label="2" v-model="ruleForm.paytype">支付宝</el-radio>
                    </el-form-item>
                    <el-form-item label="备注" prop="remark">
                        <el-input v-model="ruleForm.remark"></el-input>
                    </el-form-item>
                </div>
            </el-form>
            <div slot="footer" class="dialog-footer">
                <el-button @click="dialogFormVisible = false">取 消</el-button>
                <el-button v-if="dialogStatus=='create'" type="primary" @click="create('ruleForm')">确 定</el-button>
                <el-button v-else type="primary" @click="update('ruleForm')">确 定</el-button>
            </div>
        </el-dialog>
    </div>
</template>

<script>
import {
  BusinessrelayAmountList,
  AuditBusinessrelayAmount,
  BusinessrelayAmount
} from '@/api/shmanager'
import { SHBusinessAppinfoList } from '@/api/user.js'
import { getToken, setToken, removeToken } from '@/utils/auth'
import { validatenumber } from '@/utils/validate.js'

const statusList = [
  { key: '3', name: '全部' },
  { key: '0', name: '未处理' },
  { key: '1', name: '已处理' },
  { key: '2', name: '退回' }
]

export default {
  data() {
    var bus_change = (rule, value, callback) => {
      if (!value) {
        callback(new Error('请选择商户'))
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
      listLoading: true,
      business_pass: null,
      dialogFormVisible: false,
      dialogStatus: '',
      update_type: 1,
      sh_business_list: null,
      textMap: {
        update: '编辑',
        create: '创建'
      },
      ruleForm: {},
      listQuery: {
        page: 1,
        pagesize: 10,
        status: '3',
        begindate: this.cfg.getCurrentMonthFirst(),
        enddate: this.cfg.getCurrentMonthLast(),
        keyword: '',
        paytype: '1',
        sh_businessid: null
      },
      multipleSelection: [],
      statusList,
      rules: {
        status: [{ required: true, message: '请选择状态', trigger: 'blur' }],
        reason: [{ required: true, message: '请输入原因', trigger: 'blur' }],
        sh_businessid: [{ validator: bus_change, trigger: 'change' }],
        amount: [{ validator: number_change, trigger: 'change' }],
        remark: [{ required: true, message: '请输入备注', trigger: 'blur' }]
      }
    }
  },
  filters: {
    statusFilter(status) {
      const statusMap = {
        0: '未处理',
        1: '已处理',
        2: '退回'
      }
      return statusMap[status]
    },
    orderTypeFilter(paytype) {
      const paytypeMap = {
        1: '微信',
        2: '支付宝',
        3: '银联',
        4: '银联二维码'
      }
      return paytypeMap[paytype]
    },
    dateFilter(value) {
      if (value) {
        return value.replace('T', ' ')
      }
    }
  },
  created() {
    // SHBusinessAppinfoList('get', {
    //   page: 1,
    //   pagesize: 999,
    //   code: '',
    //   status: 2
    // }).then(response => {
    //   this.sh_business_list = response.data
    // })
    this.fetchData()
  },
  methods: {
    handleFilter() {
      this.listQuery.page = 1
      if (this.listQuery.begindate) {
        this.listQuery.begindate = this.cfg.formatDate(
          new Date(this.listQuery.begindate)
        )
      }
      if (this.listQuery.enddate) {
        this.listQuery.enddate = this.cfg.formatDate(
          new Date(this.listQuery.enddate)
        )
      }
      this.fetchData()
    },
    handleUpdate(row, update_type) {
      this.update_type = update_type
      this.ruleForm = Object.assign({}, row)
      this.dialogStatus = 'update'
      this.dialogFormVisible = true
    },
    handleBaoZ(update_type) {
      this.update_type = update_type
      this.ruleForm = Object.assign(
        {},
        {
          sh_businessid: null,
          amount: null,
          paytype: 1,
          remark: null
        }
      )
      this.dialogFormVisible = true
    },
    update(formName) {
      this.$refs[formName].validate(valid => {
        if (valid) {
          if (this.update_type == 1) {
            AuditBusinessrelayAmount('get', this.ruleForm).then(response => {
              this.dialogFormVisible = false
              this.fetchData()
              this.$notify({
                title: '成功',
                message: '处理成功',
                type: 'success',
                duration: 2000
              })
            })
          } else if (this.update_type == 2) {
            BusinessrelayAmount('get', this.ruleForm).then(response => {
              this.dialogFormVisible = false
              this.fetchData()
              this.$notify({
                title: '成功',
                message: '充值成功',
                type: 'success',
                duration: 2000
              })
            })
          }
        } else {
          console.log('error submit!!')
          return false
        }
      })
    },
    handleCurrentChange(val) {
      this.listQuery.page = val
      this.fetchData()
    },
    handleSizeChange(val) {
      this.listQuery.pagesize = val
      this.BusinessrelayAmountList()
    },
    fetchData() {
      this.listLoading = true
      BusinessrelayAmountList('get', this.listQuery).then(response => {
        this.list = response.data
        this.total = response.data ? response.data[0].count : 0

        this.listLoading = false
      })
    },
    handleSelectionChange(val) {
      this.multipleSelection = val
    },
    handleBuFa(row) {
      this.$confirm('此操作补发通知, 是否继续?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).then(() => {
        this.ruleForm = Object.assign({}, row)
        operationReissue('post', { data: this.ruleForm }).then(response => {
          this.fetchData()
          this.$notify({
            title: '成功',
            message: '补发成功',
            type: 'success',
            duration: 2000
          })
        })
      })
    },
    handleExcel() {
      var begindate = ''
      var enddate = ''
      if (this.listQuery.begindate) {
        begindate = this.listQuery.begindate
      }
      if (this.listQuery.enddate) {
        enddate = this.listQuery.enddate
      }

      window.open(
        '' +
          this.excel_api +
          'OrderExport.ashx' +
          '?status=' +
          this.listQuery.status +
          '&begindate=' +
          begindate +
          '&enddate=' +
          enddate +
          '&agid=' +
          this.listQuery.agid +
          '&merchantid=' +
          this.listQuery.merchantid +
          '&keyword=' +
          this.listQuery.keyword +
          '&businesspasstype=' +
          this.listQuery.businesspasstype
      )
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
.sumamount {
  color: red;
  font-size: 16px;
  font-weight: 500;
  margin-top: 20px;
}
.sumservicecharge {
  color: green;
  margin-left: 20px;
}
.sumrealamount {
  color: blue;
  margin-left: 20px;
}
</style>
