<template>
  <div class="app-container calendar-list-container">
    <div class="filter-container" style="position: relative;">
      <el-select class="filter-item" style="width: 130px" v-model="listQuery.status" placeholder="类型">
        <el-option v-for="item in  statusList" :key="item.key" :label="item.name" :value="item.key">
        </el-option>
      </el-select>
      <el-date-picker v-model="listQuery.begindate" type="date" placeholder="开始日期">
      </el-date-picker>
      -
      <el-date-picker v-model="listQuery.enddate" type="date" placeholder="结束日期">
      </el-date-picker>

      <el-button class="filter-item" type="primary" icon="search" @click="handleFilter">搜索</el-button>
      <el-button class="filter-item" type="primary" @click="dialogFormVisible=true">提现申请</el-button>

      <!-- <div class="sumamount" >订单金额：{{getsumamount}}</div> -->
    </div>
    <el-table :data="list" v-loading.body="listLoading" element-loading-text="拼命加载中" border fit highlight-current-row @selection-change="handleSelectionChange">
      <el-table-column type="selection" width="55">
      </el-table-column>
      <el-table-column align="center" label='序号' width="95">
        <template scope="scope">
          {{scope.$index +1}}
        </template>
      </el-table-column>
      <el-table-column label="订单号">
        <template scope="scope">
          <span>{{scope.row.despoitno}}</span>
        </template>
      </el-table-column>
      <el-table-column label="订单金额" align="center">
        <template scope="scope">
          <span>{{scope.row.amount}}</span>
        </template>
      </el-table-column>
      <el-table-column label="订单状态" align="center">
        <template scope="scope">
          <el-tag :type="(scope.row.status==1||scope.row.status==2||scope.row.status==5)?'success':''">{{scope.row.status| statusFilter}}</el-tag>
        </template>
      </el-table-column>
      <el-table-column label="代理商" align="center">
        <template scope="scope">
          {{scope.row.agname}}
        </template>
      </el-table-column>
      <el-table-column label="商户名称" align="center">
        <template scope="scope">
          {{scope.row.customername}}
        </template>
      </el-table-column>
      <el-table-column label="订单时间" align="center">
        <template scope="scope">
          {{scope.row.addtime|dateFilter}}
        </template>
      </el-table-column>
    </el-table>
    <div v-show="!listLoading" class="pagination-container">
      <el-pagination @size-change="handleSizeChange" @current-change="handleCurrentChange" :current-page.sync="listQuery.page" :page-sizes="[10,20,30, 50]" :page-size="listQuery.pagesize" layout="total, sizes, prev, pager, next, jumper" :total="total">
      </el-pagination>
    </div>

    <el-dialog :title="textMap[dialogStatus]" :visible.sync="dialogFormVisible">
      <el-form class="small-space" :model="ruleForm" :rules="rules" ref="ruleForm" label-position="left" label-width="150px" style='width: 620px; margin-left:50px;'>
        <el-form-item label="提现金额" prop="amount">
          <el-input v-model="ruleForm.amount"></el-input>
        </el-form-item>
        <el-form-item label="提现银行卡" prop="card_bankaccountNo">
          <el-select style="width: 100%" v-model="card_bankaccountNo" placeholder="选择提现银行卡">
            <el-option v-for="item in bankaccountNoList" :key="item.card_bankaccountNo" :label="item.card_bankaccountNo" :value="item.card_bankaccountNo">
            </el-option>
          </el-select>
        </el-form-item>
        <!-- <el-form-item label="提现类型" prop="paytype">
          <el-radio v-model="ruleForm.paytype" label="1">微信</el-radio>
          <el-radio v-model="ruleForm.paytype" label="2">支付宝</el-radio>
          <el-radio v-model="ruleForm.paytype" label="3">银联</el-radio>
          <el-radio v-model="ruleForm.paytype" label="4">京东</el-radio>
        </el-form-item> -->
      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button @click="dialogFormVisible = false">取 消</el-button>
        <el-button type="primary" @click="withdrawals_fun('ruleForm')">确 定 </el-button>
      </div>
    </el-dialog>
  </div>
</template>

<script>
import { getList } from '@/api/order'
import { merchantcity, businesscategory, bankcode } from '@/api/basic.js'
import { getmerchantcard, SendAuditWithdrawals } from '@/api/merchant.js'
import { getToken, setToken, removeToken } from '@/utils/auth'
import { validatenumber } from '@/utils/validate.js'
import { mapGetters } from 'vuex'

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
    var checkamount = (rule, value, callback) => {
      if (!validatenumber(value)) {
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
        pagesize: 10,
        status: '7',
        begindate: this.cfg.getCurrentMonthFirst(),
        enddate: this.cfg.getCurrentMonthLast(),
        agid: 0,
        merchantid: 0
      },
      multipleSelection: [],
      ruleForm: {
        amount: null, //提现金额
        appid: '', //appid
        card_bankaccountNo: '', //开户账号
        paytype: '2'
      },
      statusList,
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
        amount: [{ validator: checkamount, trigger: 'blur' }],
        card_bankaccountNo: [
          { validator: checkcard_bankaccountNo, trigger: 'change' }
        ]
      }
    }
  },
  computed: {
    ...mapGetters(['user_id', 'appid'])
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
    this.getbasic()
  },
  methods: {
    resetTemp() {
      this.ruleForm = {
        amount: null, //提现金额
        appid: '', //appid
        card_bankaccountNo: '', //开户账号
        paytype: '2'
      }
    },
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
    handleCurrentChange(val) {
      this.listQuery.page = val
      this.fetchData()
    },
    handleSizeChange(val) {
      this.listQuery.pagesize = val
      this.getList()
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
    fetchData() {
      this.listLoading = true
      getList('get', this.listQuery).then(response => {
        this.list = response.data
        this.total = response.data ? response.data[0].count : 0

        this.listLoading = false
      })
      // this.list = datalist.order;
      // this.total = datalist.order.length;
      // this.listLoading = false;
    },
    handleSelectionChange(val) {
      this.multipleSelection = val
    },
    withdrawals_fun(formName) {
      this.$refs[formName].validate(valid => {
        if (valid) {
          this.$confirm('发起提现申请, 是否继续?', '提示', {
            confirmButtonText: '确定',
            cancelButtonText: '取消',
            type: 'warning'
          }).then(() => {
            var req_data = {}
            req_data.customerappid = this.appid
            req_data.amount = this.ruleForm.amount
            req_data.bankno = this.card_bankaccountNo
            req_data.paytype = this.ruleForm.paytype
            req_data.businesspasstype = this.$store.getters.businesspasstype
            SendAuditWithdrawals('get', req_data)
              .then(res => {
                var status_code = res.status_code
                var message = res.message
                console.log(message)
                if (status_code == 200) {
                  this.$message({
                    message: '提现发起成功，等待收款！',
                    type: 'success',
                    duration: 5000
                  })
                  this.dialogFormVisible = false
                  this.fetchData()
                }
                // else {
                //   this.$message({
                //     message: message,
                //     type: 'error'
                //   })
                // }
              })
              .catch(res => {
                // this.$message({
                //   message: res.response.message,
                //   type: 'error'
                // })
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
.sumamount {
  position: absolute;
  right: 10px;
  top: 10px;
  color: red;
  font-size: 22px;
  font-weight: 500;
}
</style>
