<template>
  <div class="app-container calendar-list-container">
    <div class="filter-container" style="position: relative;">
      <el-input @keyup.enter.native="handleFilter" style="width: 200px;" v-model="listQuery.keyword" class="filter-item" placeholder="订单号/备注">
      </el-input>
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
      <el-select class="filter-item" style="width: 130px" v-model="listQuery.paytype" placeholder="支付类型">
        <el-option key="0" label="全部" value="0">
        </el-option>
        <el-option key="1" label="微信" value="1">
        </el-option>
        <el-option key="2" label="支付宝" value="2">
        </el-option>
      </el-select>

      <el-button class="filter-item" type="primary" icon="search" @click="handleFilter">搜索</el-button>
      <el-button class="filter-item" type="primary" @click="handleExcel">导出</el-button>

      <div class="sumamount">订单金额：{{getsumamount}}
        <span class="sumservicecharge">手续费：{{getsumservicecharge}}</span>
        <span class="sumrealamount">实到金额：{{getsumrealamount}}</span>

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
          <span>{{scope.row.orderno}}</span>
        </template>
      </el-table-column>
      <el-table-column label="订单金额" align="center">
        <template scope="scope">
          <span style="color:red">{{scope.row.amount}}</span>
        </template>
      </el-table-column>
      <el-table-column label="手续费" align="center">
        <template scope="scope">
          <span style="color:green">{{scope.row.servicecharge}}</span>
        </template>
      </el-table-column>
      <el-table-column label="实到金额" align="center">
        <template scope="scope">
          <span style="color:blue">{{scope.row.realamount}}</span>
        </template>
      </el-table-column>
      <el-table-column label="支付宝账号" align="center">
        <template scope="scope">
          <span>{{scope.row.alipayaccount}}</span>
        </template>
      </el-table-column>
      <el-table-column label="订单状态" align="center">
        <template scope="scope">
          <el-tag :type="scope.row.status==1?'success':''">{{scope.row.status| statusFilter}}</el-tag>
        </template>
      </el-table-column>
      <el-table-column label="代理商" align="center">
        <template scope="scope">
          {{scope.row.agname}}
        </template>
      </el-table-column>
      <!-- <el-table-column label="大商户" align="center">
        <template scope="scope">
          {{scope.row.merchantname}}
        </template>
      </el-table-column> -->
      <el-table-column label="商户名称" align="center">
        <template scope="scope">
          {{scope.row.shortname}}
        </template>
      </el-table-column>
      <!-- <el-table-column label="支付通道" align="center">
        <template scope="scope">
          {{scope.row.businesspasstype}}
        </template>
      </el-table-column> -->

      <el-table-column label="付款类型" align="center">
        <template scope="scope">
          <el-tag type="success">{{scope.row.paytype| orderTypeFilter}}</el-tag>
        </template>
      </el-table-column>
      <el-table-column label="付款方式" align="center">
        <template scope="scope">
          {{scope.row.auth_code}}
        </template>
      </el-table-column>
      <el-table-column label="备注" align="center">
        <template scope="scope">
          {{scope.row.remark}}
        </template>
      </el-table-column>
      <el-table-column label="订单时间" align="center">
        <template scope="scope">
          {{scope.row.addtime|dateFilter}}
        </template>
      </el-table-column>
      <el-table-column label="操作" width="100">
        <template scope="scope">
          <el-button @click.prevent="handleBuFa(scope.row)" :disabled="scope.row.status!=1" type="text" size="small">补发通知</el-button>
        </template>
      </el-table-column>
    </el-table>
    <div v-show="!listLoading" class="pagination-container">
      <el-pagination @size-change="handleSizeChange" @current-change="handleCurrentChange" :current-page.sync="listQuery.page" :page-sizes="[10,20,30, 50]" :page-size="listQuery.pagesize" layout="total, sizes, prev, pager, next, jumper" :total="total">
      </el-pagination>
    </div>
  </div>
</template>

<script>
import { getList, OrderExport } from '@/api/order'
import { getToken, setToken, removeToken } from '@/utils/auth'
import { BusinessPass } from '@/api/user.js'

const statusList = [
  { key: '0', name: '未支付' },
  { key: '1', name: '已支付' },
  { key: '2', name: '支付失败' },
  { key: '3', name: '全部' }
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
        status: '1',
        begindate: this.cfg.getCurrentMonthFirst(),
        enddate: this.cfg.getCurrentMonthLast(),
        agid: 0,
        merchantid: 0,
        keyword: '',
        businesspasstype: '',
        paytype: '0'
      },
      multipleSelection: [],
      statusList
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
    getsumservicecharge() {
      if (this.list != null && this.list.length > 0) {
        return this.list[0].sumservicecharge
      } else {
        return 0
      }
    },
    getsumrealamount() {
      if (this.list != null && this.list.length > 0) {
        return this.list[0].sumrealamount
      } else {
        return 0
      }
    }
  },
  filters: {
    statusFilter(status) {
      const statusMap = {
        0: '未支付',
        1: '支付完成',
        2: '支付失败'
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
    handleCurrentChange(val) {
      this.listQuery.page = val
      this.fetchData()
    },
    handleSizeChange(val) {
      this.listQuery.pagesize = val
      this.getList()
    },
    fetchData() {
      this.listLoading = true
      getList('get', this.listQuery).then(response => {
        this.list = response.data
        this.total = response.data ? response.data[0].count : 0

        this.listLoading = false
      })
      BusinessPass('get').then(response => {
        this.business_pass = response.data
      })
      // this.list = datalist.order;
      // this.total = datalist.order.length;
      // this.listLoading = false;
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
