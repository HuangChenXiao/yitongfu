<template>
  <div class="app-container calendar-list-container">
    <div class="filter-container">
      <el-select v-model="listQuery.status" filterable placeholder="提现状态" class="filter-item" >
      <el-option label="未审核" value="0"></el-option>
      <el-option label="审核通过" value="1"></el-option>
      <el-option label="审核未通过" value="2"></el-option>
      <el-option label="全部" value="3"></el-option>
    </el-select>
    <el-date-picker
    v-model="listQuery.begindate"
    type="date"
    placeholder="开始日期">
  </el-date-picker>
  -
  <el-date-picker
  v-model="listQuery.enddate"
  type="date"
  placeholder="结束日期">
</el-date-picker>
<el-button class="filter-item" type="primary" icon="search" @click="handleFilter">搜索</el-button>
<el-button class="filter-item" type="primary" @click="handleUpdate">提现申请</el-button>
</div>
<el-table :data="list" v-loading.body="listLoading" element-loading-text="拼命加载中" border fit highlight-current-row> 
<el-table-column align="center" label='序号' width="95">
<template scope="scope">
  {{scope.$index}}
</template>
</el-table-column>
<!-- <el-table-column label="提现前佣金"  align="center">
<template scope="scope">
  <span>{{scope.row.lastcommission}}</span>
</template>
</el-table-column>
<el-table-column label="提现后佣金"  align="center">
<template scope="scope">
  <span>{{scope.row.newcommission}}</span>
</template>
</el-table-column> -->
<el-table-column label="本次提现佣金"  align="center">
<template scope="scope">
  <span>{{scope.row.commission}}</span>
</template>
</el-table-column>
<el-table-column label="状态"  align="center">
<template scope="scope">
  <!-- <span>{{scope.row.status|statusFilter}}</span> -->
  <el-tag :type="scope.row.status | tagFilter">{{scope.row.status|statusFilter}}</el-tag>
</template>
</el-table-column>
<el-table-column label="原因"  align="center">
<template scope="scope">
  <span>{{scope.row.reason}}</span>
</template>
</el-table-column>
<el-table-column label="提现人"  align="center">
<template scope="scope">
  <span>{{scope.row.adduser}}</span>
</template>
</el-table-column>
<el-table-column label="提现时间"  align="center">
<template scope="scope">
  <span>{{scope.row.addtime|dateFilter}}</span>
</template>
</el-table-column>
<el-table-column label="审核人"  align="center">
        <template scope="scope">
          <span>{{scope.row.updateuser}}</span>
        </template>
      </el-table-column>
      <el-table-column label="审核时间"  align="center">
        <template scope="scope">
          <span>{{scope.row.updatetime|dateFilter}}</span>
        </template>
      </el-table-column>
      <!-- <el-table-column
        fixed="right"
        label="操作"
        width="80">
        <template scope="scope">
          <el-button @click.prevent="handleUpdate(scope.row)" :disabled="scope.row.status!=0" type="primary"  size="small">审核</el-button>
        </template>
      </el-table-column> -->
    </el-table>

    <div v-show="!listLoading" class="pagination-container">
      <el-pagination @size-change="handleSizeChange" @current-change="handleCurrentChange" :current-page.sync="listQuery.page"
      :page-sizes="[10,20,30, 50]" :page-size="listQuery.pagesize" layout="total, sizes, prev, pager, next, jumper" :total="total">
    </el-pagination>
  </div>

  <el-dialog :title="textMap[dialogStatus]" :visible.sync="dialogFormVisible">
  <el-form class="small-space" :model="ruleForm" :rules="rules" ref="ruleForm" label-position="left" label-width="150px" style='width: 620px; margin-left:50px;'>
  <el-form-item label="提现金额" prop="commission">
  <el-input v-model.number="ruleForm.commission" placeholder="请输入提现金额"></el-input>
</el-form-item>
</el-form>

<div slot="footer" class="dialog-footer">
  <el-button @click="dialogFormVisible=false">取 消</el-button>
  <el-button type="primary" @click="apply('ruleForm')">发起申请</el-button>
</div>
</el-dialog>
</div>
</template>

<script>
import { withdrawalsbyagid, editwithdrawalsbyagid } from "@/api/withdrawals.js";

export default {
  data() {
    var checkcommission = (rule, value, callback) => {
      if (!Number.parseFloat(value)||Number.parseFloat(value)<=0) {
        callback(new Error("请输入有效的佣金"));
      } else {
        if(Number.parseFloat(value)<100){
          callback(new Error("单次提现金额不能小于100"));
        }
        else{
          callback();
        }
      }
    };
    return {
      list: null,
      total: null,
      listLoading: true,
      listQuery: {
        status: "3",
        page: 1,
        pagesize: 20,
        begindate: this.cfg.getCurrentMonthFirst(),
        enddate: this.cfg.getCurrentMonthLast(),
      },
      business_category: [], //经营类目
      bank_code: [], //银行代码
      business_basic: [], //商户列表
      ruleForm: "",
      dialogFormVisible: false,
      dialogStatus: "",
      textMap: {
        apply: "提现申请"
      },
      dialogPvVisible: false,
      rules: {
        commission: [{ validator: checkcommission, trigger: "blur" }]
      }
    };
  },
  computed: {},
  filters: {
    tagFilter(status) {
      const statusMap = {
        1: "success",
        2: "danger",
        0: "warning"
      };
      return statusMap[status];
    },
    statusFilter(status) {
      const statusMap = {
        1: "审核通过",
        2: "审核未通过",
        0: "未审核"
      };
      return statusMap[status];
    },
    dateFilter(value) {
      if (value) {
        return value.replace("T", " ");
      }
    }
  },
  created() {
    this.fetchData();
  },
  methods: {
    resetTemp() {
      this.ruleForm = {
        commission: "", //id
      };
    },
    handleFilter() {
      this.listQuery.page = 1;
      this.fetchData();
    },
    handleCurrentChange(val) {
      this.listQuery.page = val;
      this.fetchData();
    },
    handleSizeChange(val) {
      this.listQuery.pagesize = val;
      this.getList();
    },
    handleUpdate(row) {
      this.ruleForm = Object.assign({}, row);
      this.dialogStatus = "apply";
      this.dialogFormVisible = true;
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
      this.listLoading = true;
      if(this.listQuery.begindate){
        this.listQuery.begindate=this.cfg.formatDate(new Date(this.listQuery.begindate))
      }
      if(this.listQuery.enddate){
        this.listQuery.enddate=this.cfg.formatDate(new Date(this.listQuery.enddate))
      }
      withdrawalsbyagid("get", this.listQuery).then(response => {
        this.list = response.data;
        this.total = response.total;

        this.listLoading = false;
      });
      // console.log(bData);
      // this.list = bData.router;
      // this.total = bData.router.length;
      // this.listLoading = false;
    },
    refuse(formName) {
      this.$refs[formName].validate(valid => {
        if (valid) {
          this.$confirm("拒绝提现申请, 是否继续?", "提示", {
            confirmButtonText: "确定",
            cancelButtonText: "取消",
            type: "warning"
          }).then(() => {
            this.ruleForm.status = 2;
            this.editwithdrawals();
          })
        } else {
          console.log("error submit!!");
          return false;
        }
      });
    },
    apply(formName) {
      this.$refs[formName].validate(valid => {
        if (valid) {
          this.$confirm("每个月只能发送一次佣金提现申请, 是否继续?", "提示", {
            confirmButtonText: "确定",
            cancelButtonText: "取消",
            type: "warning"
          }).then(() => {
            editwithdrawalsbyagid("put", this.ruleForm).then(response => {
              this.dialogFormVisible = false;
              this.fetchData();
              this.resetTemp();
              this.$notify({
                title: "成功",
                message: "提现申请成功，等待后台审核！",
                type: "success",
                duration: 5000
              });
            });
          })
        } else {
          console.log("error submit!!");
          return false;
        }
      });
    }
  },
  watch: {
    dialogFormVisible(val, oldValue) {
      if (!val) {
        this.$refs["ruleForm"].resetFields();
      }
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
.el-tag + .el-tag {
  margin-left: 10px;
}
</style>
