<template>
  <div class="app-container calendar-list-container">
    <div class="filter-container" style="position: relative;">
      <el-date-picker v-model="listQuery.begindate" type="date" placeholder="开始日期">
      </el-date-picker>
      -
      <el-date-picker v-model="listQuery.enddate" type="date" placeholder="结束日期">
      </el-date-picker>

      <el-button class="filter-item" type="primary" icon="search" @click="handleFilter">搜索</el-button>
    </div>
    <el-table :data="list" v-loading.body="listLoading" element-loading-text="拼命加载中" border fit highlight-current-row @selection-change="handleSelectionChange">
      <el-table-column align="center" label='序号' width="95">
        <template scope="scope">
          {{scope.$index +1}}
        </template>
      </el-table-column>
      <el-table-column label="商户名称">
        <template scope="scope">
          <span>{{scope.row.shortname}}</span>
        </template>
      </el-table-column>
      <el-table-column label="支付宝账号" align="center">
        <template scope="scope">
          <span>{{scope.row.alipayaccount}}</span>
        </template>
      </el-table-column>
      <el-table-column label="成功笔数" align="center">
        <template scope="scope">
          <span>{{scope.row.paycount}}</span>
        </template>
      </el-table-column>
      <el-table-column label="未成功笔数" align="center">
        <template scope="scope">
          <span>{{scope.row.unpaycount}}</span>
        </template>
      </el-table-column>
      <el-table-column label="金额" align="center">
        <template scope="scope">
          <span>{{scope.row.amount}}</span>
        </template>
      </el-table-column>
      <el-table-column label="手续费" align="center">
        <template scope="scope">
          <span>{{scope.row.servicecharge}}</span>
        </template>
      </el-table-column>
      <el-table-column label="实到金额" align="center">
        <template scope="scope">
          <span>{{scope.row.realamount}}</span>
        </template>
      </el-table-column>
      
      
      <!-- <el-table-column fixed="right" label="操作" width="80">
        <template scope="scope">
          <el-button @click.prevent="handleUpdate(scope.row)" v-if="scope.row.status==1" type="primary" size="small">审核</el-button>
          <el-button v-else :disabled="scope.row.status!=1" type="primary" size="small">审核</el-button>
        </template>
      </el-table-column> -->
    </el-table>
    <div v-show="!listLoading" class="pagination-container">
      <el-pagination @size-change="handleSizeChange" @current-change="handleCurrentChange" :current-page.sync="listQuery.page" :page-sizes="[10,20,30, 50]" :page-size="listQuery.pagesize" layout="total, sizes, prev, pager, next, jumper" :total="total">
      </el-pagination>
    </div>

    <el-dialog :title="textMap[dialogStatus]" :visible.sync="dialogFormVisible">
      <el-form class="small-space" :model="ruleForm" :rules="rules" ref="ruleForm" label-position="left" label-width="150px" style='width: 620px; margin-left:50px;'>
        <el-form-item label="原因" prop="reason">
          <el-input v-model="ruleForm.reason" placeholder="拒绝必须输入原因"></el-input>
        </el-form-item>
      </el-form>

      <div slot="footer" class="dialog-footer">
        <el-button type="danger" @click="refuse('ruleForm')">拒 绝</el-button>
        <el-button type="primary" @click="audit()">通 过</el-button>
      </div>
    </el-dialog>
  </div>
</template>

<script>
import { PayAccountOrderAmount } from '@/api/report'
import { getToken, setToken, removeToken } from '@/utils/auth'

const statusList = [
  { key: '7', name: '全部' },
  { key: '0', name: '提现中' },
  { key: '1', name: '代理商审核' },
  { key: '2', name: '总部审核' },
  { key: '3', name: '审核不通过' },
  { key: '4', name: '代付成功中' },
  { key: '5', name: '代付成功' },
  { key: '6', name: '代付失败' }
]

export default {
  data() {
    return {
      list: null,
      total: null,
      listLoading: true,
      business_pass: null,
      listQuery: {
        page: 1,
        pagesize: 10,
        keyword: '',
        begindate: this.cfg.getCurrentMonthFirst(),
        enddate: this.cfg.getCurrentMonthLast(),
      },
      multipleSelection: [],
      statusList,
      ruleForm: {},
      dialogFormVisible: false,
      dialogStatus: '',
      textMap: {
        audit: '提现审核'
      },
      dialogPvVisible: false,
      rules: {
        reason: [{ required: true, message: '请输入拒绝原因', trigger: 'blur' }]
      }
    }
  },
  computed: {
    getsumamount() {
      if (this.list != null && this.list.length > 0) {
        return this.list[0].sumamount
      } else {
        return 0
      }
    }
  },
  filters: {
    statusFilter(status) {
      const statusMap = {
        0: '提现中',
        1: '代理商审核',
        2: '总部审核',
        3: '审核不通过',
        4: '代付成功中',
        5: '代付成功',
        6: '代付失败'
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
          'PutForwardOrder.ashx' +
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
    },
    handleCurrentChange(val) {
      this.listQuery.page = val
      this.fetchData()
    },
    handleSizeChange(val) {
      this.listQuery.pagesize = val
      this.presentrecord()
    },
    handleUpdate(row) {
      this.ruleForm = Object.assign({}, row)
      this.dialogStatus = 'audit'
      this.dialogFormVisible = true
    },
    fetchData() {
      this.listLoading = true
      PayAccountOrderAmount('get', this.listQuery).then(response => {
        this.list = response.data
        this.total = response.data ? response.data[0].count : 0

        this.listLoading = false
      })
    },
    handleSelectionChange(val) {
      this.multipleSelection = val
    },
    refuse(formName) {
      this.$refs[formName].validate(valid => {
        if (valid) {
          this.$confirm('拒绝提现申请, 是否继续?', '提示', {
            confirmButtonText: '确定',
            cancelButtonText: '取消',
            type: 'warning'
          }).then(() => {
            this.ruleForm.status = 3
            this.editwithdrawals()
          })
        } else {
          console.log('error submit!!')
          return false
        }
      })
    },
    audit() {
      this.$confirm('通过提现申请, 是否继续?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).then(() => {
        this.ruleForm.status = 5
        this.editwithdrawals()
      })
    },
    editwithdrawals() {
      var jdata = {}
      jdata.despoitno = this.ruleForm.despoitno
      jdata.status = this.ruleForm.status
      jdata.reason = this.ruleForm.reason
      withdrawPayAPI('post', { data: jdata }).then(response => {
        if (response.data.statue == 1) {
          this.dialogFormVisible = false
          this.fetchData()
          this.$notify({
            title: '成功',
            message: '操作成功',
            type: 'success',
            duration: 2000
          })
        } else {
          this.$message({
            message: response.data.message,
            type: 'warning'
          })
        }
      })
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
  position: absolute;
  right: 10px;
  top: 10px;
  color: red;
  font-size: 22px;
  font-weight: 500;
}
</style>
