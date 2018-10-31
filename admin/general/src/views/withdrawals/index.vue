<template>
  <div class="app-container calendar-list-container">
    <div class="filter-container">
      <el-input @keyup.enter.native="handleFilter" v-authority="{ color: 'white', text: 'hello!' }" style="width: 200px;" class="filter-item" placeholder="代理商" v-model="listQuery.keyword">
      </el-input>
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
    </div>
    <el-table :data="list" v-loading.body="listLoading" element-loading-text="拼命加载中" border fit highlight-current-row> 
      <el-table-column align="center" label='序号' width="95">
        <template scope="scope">
          {{scope.$index}}
        </template>
      </el-table-column>
      <el-table-column label="代理商"  align="center">
        <template scope="scope">
          <span>{{scope.row.agname}}</span>
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
      <el-table-column
        fixed="right"
        label="操作"
        width="100">
        <template scope="scope">
          <el-button @click.prevent="handleUpdate(scope.row)" v-if="scope.row.status==0" type="primary"  size="small">审核</el-button>
          <el-button v-else :disabled="scope.row.status!=0" type="primary"  size="small">审核</el-button>
          <!-- <el-button @click.prevent="handleDelete(scope.row)" type="text" size="small">删除</el-button> -->
        </template>
      </el-table-column>
    </el-table>

    <div v-show="!listLoading" class="pagination-container">
      <el-pagination @size-change="handleSizeChange" @current-change="handleCurrentChange" :current-page.sync="listQuery.page"
        :page-sizes="[10,20,30, 50]" :page-size="listQuery.pagesize" layout="total, sizes, prev, pager, next, jumper" :total="total">
      </el-pagination>
    </div>

    <el-dialog :title="textMap[dialogStatus]" :visible.sync="dialogFormVisible">
      <el-form class="small-space" :model="ruleForm" :rules="rules" ref="ruleForm" label-position="left" label-width="150px" style='width: 620px; margin-left:50px;'>
        <el-form-item label="原因" prop="reason">
          <el-input v-model="ruleForm.reason" placeholder="拒绝必须输入原因"></el-input>
        </el-form-item>
      </el-form>
      
      <div slot="footer" class="dialog-footer">
        <el-button type="danger"  @click="refuse('ruleForm')">拒 绝</el-button>
        <el-button type="primary" @click="audit()">通 过</el-button>
      </div>
    </el-dialog>
  </div>
</template>

<script>
import { withdrawals, editwithdrawals } from "@/api/withdrawals.js";

export default {
  data() {
    var validatePass = (rule, value, callback) => {
      //密码
      if (value === "" && this.dialogStatus == "create") {
        callback(new Error("请输入密码"));
      } else {
        callback();
      }
    };
    return {
      list: null,
      total: null,
      listLoading: true,
      listQuery: {
        begindate: this.cfg.getCurrentMonthFirst(),
        enddate: this.cfg.getCurrentMonthLast(),
        status: "3",
        keyword: "",
        page: 1,
        pagesize: 20
      },
      business_category: [], //经营类目
      bank_code: [], //银行代码
      business_basic: [], //商户列表
      ruleForm: "",
      dialogFormVisible: false,
      dialogStatus: "",
      textMap: {
        audit: "提现审核"
      },
      dialogPvVisible: false,
      rules: {
        reason: [{ required: true, message: "请输入拒绝原因", trigger: "blur" }]
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
        id: "", //id
        merchantId: "", //商户编号
        mobileNo: "", //持卡人手机号
        handleType: "0", //操作类型
        bankCode: "", //银行代码
        bankaccProp: "0", //账户属性
        name: "", //持卡人姓名
        bankaccountType: "1", //银行卡类型 1-借记卡（账户属性为0-私人时，只能上送1）；2-贷记卡；3-存折
        bankaccountNo: "", //银行卡号
        certCode: "", //办卡证件类型
        certNo: "", //证件号码
        bankbranchNo: "", //联行号
        defaultAcc: "1", //默认账户
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
      this.dialogStatus = "audit";
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
      withdrawals("get", this.listQuery).then(response => {
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
    audit() {
      this.$confirm("通过提现申请, 是否继续?", "提示", {
        confirmButtonText: "确定",
        cancelButtonText: "取消",
        type: "warning"
      }).then(() => {
        this.ruleForm.status = 1;
        this.editwithdrawals();
      });
    },
    editwithdrawals() {
      editwithdrawals("put", this.ruleForm).then(response => {
        this.dialogFormVisible = false;
        this.fetchData();
        this.$notify({
          title: "成功",
          message: "操作成功",
          type: "success",
          duration: 2000
        });
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
