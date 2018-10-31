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
      <el-table-column label="订单金额" align="center">
        <template scope="scope">
          <span style="color:red">{{scope.row.amount}}</span>
        </template>
      </el-table-column>
      <el-table-column label="收款金额" align="center">
        <template scope="scope">
          <span style="color:red">{{scope.row.realamount}}</span>
        </template>
      </el-table-column>
      <el-table-column label="手续费" align="center">
        <template scope="scope">
          <span style="color:green">{{scope.row.servicecharge}}</span>
        </template>
      </el-table-column>
      <el-table-column label="支付通道" align="center">
        <template scope="scope">
          {{scope.row.businesspasstype}}
        </template>
      </el-table-column>

      <el-table-column label="付款类型" align="center">
        <template scope="scope">
          <el-tag type="success">{{scope.row.paytype| orderTypeFilter}}</el-tag>
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
import { OrderSumList } from '@/api/order'
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
        begindate: this.cfg.getCurrentMonthFirst(),
        enddate: this.cfg.getCurrentMonthLast(),
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
        3: 'QQ钱包',
        4: '京东'
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
      this.OrderSumList()
    },
    fetchData() {
      this.listLoading = true
      OrderSumList('get', this.listQuery).then(response => {
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
