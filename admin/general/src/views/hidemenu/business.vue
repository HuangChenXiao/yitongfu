<template>
  <div class="app-container calendar-list-container">
    <!-- <div class="filter-container">
      <el-input @keyup.enter.native="handleFilter" style="width: 200px;" class="filter-item" placeholder="用户编号/名称" v-model="listQuery.code">
      </el-input>

      <el-button class="filter-item" type="primary" icon="search" @click="handleFilter">搜索</el-button>
      <el-button class="filter-item" style="margin-left: 10px;" @click="handleCreate" type="primary" icon="edit">添加</el-button>
    </div> -->
    <el-table :data="list" v-loading.body="listLoading" element-loading-text="拼命加载中" border fit highlight-current-row> 
      <el-table-column align="center" label='序号' width="95">
        <template scope="scope">
          {{scope.$index+1}}
        </template>
      </el-table-column>
      <el-table-column label="商户名称"  align="center">
        <template scope="scope">
          <span>{{scope.row.shortname}}</span>
        </template>
      </el-table-column>
      <el-table-column label="商户地址"  align="center">
        <template scope="scope">
          <span>{{scope.row.merchantaddress}}</span>
        </template>
      </el-table-column>
      <el-table-column label="手机号"  align="center">
        <template scope="scope">
          <span>{{scope.row.servicephone}}</span>
        </template>
      </el-table-column>
      <el-table-column label="开户人"  align="center">
        <template scope="scope">
          <span>{{scope.row.corpmanname}}</span>
        </template>
      </el-table-column>
      <el-table-column label="余额"  align="center">
        <template scope="scope">
          <span>{{scope.row.balance}}</span>
        </template>
      </el-table-column>
      <el-table-column label="订单金额"  align="center">
        <template scope="scope">
          <span>{{scope.row.orderamount}}</span>
        </template>
      </el-table-column>
      <el-table-column label="提现金额"  align="center">
        <template scope="scope">
          <span>{{scope.row.despoitamount}}</span>
        </template>
      </el-table-column>
    </el-table>
  </div>
</template>

<script>
import { Merchantbusiness } from "@/api/user.js";
export default {
  data() {
    return {
      list: null,
      listLoading: true,
      listQuery: {
        merchantid: this.$route.params.merchantid
      },
    };
  },
  filters: {
    statusFilter(status) {
      const statusMap = {
        1: "启用",
        2: "禁用"
      };
      return statusMap[status];
    }
  },
  created() {
    this.fetchData();
  },
  methods: {
    fetchData() {
      this.listLoading = true;
      Merchantbusiness("get", this.listQuery).then(response => {
        this.list = response.data;
        this.listLoading = false;
      });
    }
  }
};
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
