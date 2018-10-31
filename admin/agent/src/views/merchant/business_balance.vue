<template>
  <div class="app-container calendar-list-container">
    <div class="filter-container">
      <el-input @keyup.enter.native="handleFilter" style="width: 200px;" class="filter-item" placeholder="商户名称" v-model="listQuery.code">
      </el-input>

      <el-button class="filter-item" type="primary" icon="search" @click="handleFilter">搜索</el-button>
      <!-- <el-button class="filter-item" style="margin-left: 10px;" @click="handleCreate" type="primary" icon="edit">添加</el-button> -->
    </div>
    <el-table :data="list" v-loading.body="listLoading" element-loading-text="拼命加载中" border fit highlight-current-row> 
      <el-table-column align="center" label='序号' width="95">
        <template scope="scope">
          {{scope.$index+1}}
        </template>
      </el-table-column>
      <el-table-column label="商户名称"  align="center">
        <template scope="scope">
          <span>{{scope.row.shortName}}</span>
        </template>
      </el-table-column>
      <el-table-column label="微信余额"  align="center">
        <template scope="scope">
          <span>{{scope.row.wechatbalance}}</span>
        </template>
      </el-table-column>
      <el-table-column label="支付宝余额"  align="center">
        <template scope="scope">
          <span>{{scope.row.alipaybalance}}</span>
        </template>
      </el-table-column>
      <el-table-column label="总余额"  align="center">
        <template scope="scope">
          <span>{{scope.row.totalbalance}}</span>
        </template>
      </el-table-column>
    </el-table>

    <div v-show="!listLoading" class="pagination-container">
      <el-pagination @size-change="handleSizeChange" @current-change="handleCurrentChange" :current-page.sync="listQuery.page"
        :page-sizes="[10,20,30, 50]" :page-size="listQuery.pagesize" layout="total, sizes, prev, pager, next, jumper" :total="total">
      </el-pagination>
    </div>

    <el-dialog :title="textMap[dialogStatus]" :visible.sync="dialogFormVisible">
      <el-form class="small-space" :model="ruleForm" :rules="rules" ref="ruleForm"  label-position="right" label-width="100px" style='width: 400px; margin-left:50px;'>
        <el-form-item label="角色" prop="roleid">
          <el-select v-model="ruleForm.roleid" filterable placeholder="请选择用户角色"  style="width:100%">
          <el-option v-if="!item.isadmin"
            v-for="item in selectlist"
            :key="item.roleid"
            :label="item.rolename"
            :value="item.roleid">
          </el-option>
        </el-select>
        </el-form-item>
        <el-form-item label="登录账号" prop="usercode">
          <el-input v-model="ruleForm.usercode" :disabled="dialogStatus=='update'"></el-input>
        </el-form-item>
        <el-form-item label="账号名称" prop="username">
          <el-input v-model="ruleForm.username"></el-input>
        </el-form-item>
        <el-form-item label="密码">
          <el-input v-model="ruleForm.password"></el-input>
        </el-form-item>
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
import { BusBalance } from "@/api/merchant.js";
export default {
  data() {
    return {
      list: null,
      total: null,
      selectlist: null,
      listLoading: true,
      listQuery: {
        page: 1,
        pagesize: 10,
        code: "",
        agentid:this.$store.getters.userid,
        businessid:0
      },
      ruleForm: {
        userid: "",
        roleid: null,
        usercode: "",
        username: "",
        password: "",
      },
      dialogFormVisible: false,
      dialogStatus: "",
      textMap: {
        update: "编辑",
        create: "创建"
      },
      dialogPvVisible: false,
      pvData: [],
      rules: {
        usercode: [{ required: true, message: "请输入登录账号", trigger: "blur" }],
        username: [{ required: true, message: "请输入账号名称", trigger: "blur" }],
        // roleid: [
        //     { required: true, message: '请选择用户角色', trigger: 'change' }
        // ],
      }
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
    handleCreate() {
      this.resetTemp();
      this.dialogStatus = "create";
      this.dialogFormVisible = true;
    },
    handleUpdate(row) {
      this.ruleForm = Object.assign({}, row);
      this.ruleForm.password="";
      this.dialogStatus = "update";
      this.dialogFormVisible = true;
    },
    resetTemp() {
      this.ruleForm = {
        userid: "",
        roleid: null,
        usercode: "",
        username: "",
        password: "",
      };
    },
    fetchData() {
      this.listLoading = true;
      BusBalance("get", this.listQuery).then(response => {
        this.list = response.data;
        this.total = response.data.length > 0 ? response.data.length : 0;

        this.listLoading = false;
      });
    },
    selectRoleList() {
      role("get").then(response => {
        this.selectlist = response.data;
        console.log(this.selectlist);
      });
    },
    update(formName) {
      this.$refs[formName].validate(valid => {
        if (valid) {
          editadmin("put",this.ruleForm )
            .then(response => {
              this.dialogFormVisible = false;
              this.fetchData();
              this.$notify({
                title: "成功",
                message: "更新成功",
                type: "success",
                duration: 2000
              });
            })
        } else {
          console.log("error submit!!");
          return false;
        }
      });
    },
    create(formName) {
      this.$refs[formName].validate(valid => {
        if (valid) {
          editadmin("post",this.ruleForm)
            .then(response => {
              this.dialogFormVisible = false;
              this.fetchData();
              this.$notify({
                title: "成功",
                message: "新增成功",
                type: "success",
                duration: 2000
              });
            })
        } else {
          console.log("error submit!!");
          return false;
        }
      });
    },
    handleSelectionChange(val) {
      this.multipleSelection = val;
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
