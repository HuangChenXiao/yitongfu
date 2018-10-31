<template>
  <div class="app-container calendar-list-container">
    <div class="filter-container" style="position: relative;">
      <el-select class="filter-item" style="width: 130px" v-model="listQuery.status" placeholder="类型">
        <el-option v-for="item in  statusList" :key="item.key" :label="item.name" :value="item.key">
        </el-option>
      </el-select>
      <!-- <el-select class="filter-item" v-model="listQuery.businesspasstype" placeholder="支付通道">
        <el-option v-for="item in  business_pass" :key="item.type" :label="item.memo" :value="item.type" v-if="item.enable">
        </el-option>
      </el-select> -->
      <el-date-picker v-model="listQuery.begindate" type="date" placeholder="开始日期">
      </el-date-picker>
      -
      <el-date-picker v-model="listQuery.enddate" type="date" placeholder="结束日期">
      </el-date-picker>

      <el-button class="filter-item" type="primary" icon="search" @click="handleFilter">搜索</el-button>

      <div class="sumamount">佣金金额：{{getsumamount}}
        <span class="sumservicecharge">手续费：{{getsumcharge}}</span>

      </div>
    </div>
    <el-table :data="list" v-loading.body="listLoading" element-loading-text="拼命加载中" border fit highlight-current-row @selection-change="handleSelectionChange">

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
      <el-table-column label="手续费" align="center">
        <template scope="scope">
          <span>{{scope.row.charge}}</span>
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
      <!-- <el-table-column label="虚拟商户" align="center">
        <template scope="scope">
          {{scope.row.merchantname}}
        </template>
      </el-table-column> -->
      <el-table-column label="商户名称" align="center">
        <template scope="scope">
          {{scope.row.customername}}
        </template>
      </el-table-column>
      <el-table-column label="原因" align="center">
        <template scope="scope">
          {{scope.row.reason}}
        </template>
      </el-table-column>
      <el-table-column label="订单时间" align="center">
        <template scope="scope">
          {{scope.row.addtime|dateFilter}}
        </template>
      </el-table-column>
      <el-table-column label="操作" width="80">
        <template scope="scope">
          <el-button @click.prevent="handleUpdate(scope.row)" type="text" size="small" :disabled="scope.row.status!=0">审核</el-button>
        </template>
      </el-table-column>
    </el-table>
    <div v-show="!listLoading" class="pagination-container">
      <el-pagination @size-change="handleSizeChange" @current-change="handleCurrentChange" :current-page.sync="listQuery.page" :page-sizes="[10,20,30, 50]" :page-size="listQuery.pagesize" layout="total, sizes, prev, pager, next, jumper" :total="total">
      </el-pagination>
    </div>

    <el-dialog title="提现审核" :visible.sync="dialogFormVisible">
      <el-form class="small-space" :model="ruleForm" :rules="rules" ref="ruleForm" label-position="left" label-width="150px" style='width: 620px; margin-left:50px;'>
        <el-form-item label="订单号" prop="despoitno">
          <el-input v-model="ruleForm.despoitno" disabled></el-input>
        </el-form-item>
        <el-form-item label="提现银行卡" prop="card_bankaccountNo">
          <el-radio v-model="ruleForm.status" label="1">提现成功</el-radio>
          <el-radio v-model="ruleForm.status" label="3">审核不通过</el-radio>
        </el-form-item>
        <el-form-item label="备注" prop="reason">
          <el-input v-model="ruleForm.reason" ></el-input>
        </el-form-item>
      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button @click="dialogFormVisible = false">取 消</el-button>
        <el-button type="primary" @click="withdrawals_fun('ruleForm')">确 定 </el-button>
      </div>
    </el-dialog>
  </div>
</template>

<script>
import { AgPresentRecord,auditWithdrawals,BusinessPass } from '@/api/order'
import { getToken, setToken, removeToken } from '@/utils/auth'
import { validatenumber } from '@/utils/validate.js'

const statusList = [
  { key: '7', name: '全部' },
  { key: '0', name: '提现中' },
  { key: '1', name: '代理商审核' },
  { key: '2', name: '总部审核' },
  { key: '3', name: '审核不通过' },
  { key: '4', name: '代付成功中' },
  { key: '5', name: '代付成功' },
  { key: '6', name: '代付失败' },
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
        merchantid: 0,
        businesspasstype:null
      },
      multipleSelection: [],
      statusList,
      dialogFormVisible: false,
      business_pass:null,
      ruleForm: {
        amount: null,
        charge: null,
        count: null,
        customername: null,
        despoitno: null,
        merchantname: null,
        reason: null,
        status: 1,
        sumamount: null,
        sumcharge: null
      },
      rules: {
        amount: [{ validator: checkamount, trigger: 'blur' }]
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
    },
    getsumcharge() {
      if (this.list != null && this.list.length > 0) {
        return this.list[0].sumcharge
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
        6: '代付失败',
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
    handleUpdate(row) {
      this.ruleForm = Object.assign({}, row)
      this.ruleForm.password = ''
      this.dialogStatus = 'update'
      this.dialogFormVisible = true
    },
    handleCurrentChange(val) {
      this.listQuery.page = val
      this.fetchData()
    },
    handleSizeChange(val) {
      this.listQuery.pagesize = val
      this.AgPresentRecord()
    },
    withdrawals_fun(formName) {
      this.$refs[formName].validate(valid => {
        if (valid) {
          this.$confirm("发起提现申请, 是否继续?", "提示", {
            confirmButtonText: "确定",
            cancelButtonText: "取消",
            type: "warning"
          }).then(() => {
            var idata={}
            idata.despoitno=this.ruleForm.despoitno
            idata.status=this.ruleForm.status
            idata.reason=this.ruleForm.reason
            auditWithdrawals("get", idata)
              .then(res => {
                var status_code = res.status_code;
                var message = res.message; 
                if (status_code == 200) {
                  this.$message({
                    message: "操作成功",
                    type: "success",
                    duration: 5000
                  });
                  this.dialogFormVisible = false;
                  this.fetchData();
                } else {
                  this.$message({
                    message: message,
                    type: "error"
                  });
                }
              })
              .catch(res => {
                this.$message({
                  message: res.response.data.message,
                  type: "error"
                });
              });
          });
        } else {
          console.log("error submit!!");
          return false;
        }
      });
    },
    fetchData() {
      this.listLoading = true
      AgPresentRecord('get', this.listQuery).then(response => {
        this.list = response.data
        this.total = response.data ? response.data[0].count : 0

        this.listLoading = false
      })
      BusinessPass('get').then(response => {
        this.business_pass = response.data
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
.sumamount {
  position: absolute;
  right: 10px;
  top: 10px;
  color: red;
  font-size: 16px;
  font-weight: 500;
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
