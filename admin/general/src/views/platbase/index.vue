<template>
  <div class="app-container calendar-list-container">
    <el-table :data="list" v-loading.body="listLoading" element-loading-text="拼命加载中" border fit highlight-current-row> 
      <el-table-column align="center" label='序号' width="95">
        <template scope="scope">
          {{scope.$index+1}}
        </template>
      </el-table-column>
      <el-table-column label="渠道手续费"  align="center">
        <template scope="scope">
          <span class="link-type" @click="handleUpdate(scope.row)">{{scope.row.platrate}}</span>
        </template>
      </el-table-column>
      <el-table-column label="浮动手续费"  align="center">
        <template scope="scope">
          <span>{{scope.row.floatinrate}}</span>
        </template>
      </el-table-column>
      <el-table-column align="center" prop="created_at" label="修改人">
        <template scope="scope">
          <span>{{scope.row.updateuser}}</span>
        </template>
      </el-table-column>
      <el-table-column align="center" prop="created_at" label="修改时间">
        <template scope="scope">
          <i class="el-icon-time"></i>
          <span>{{scope.row.updatetime}}</span>
        </template>
      </el-table-column>
    </el-table>

    <el-dialog :title="textMap[dialogStatus]" :visible.sync="dialogFormVisible">
      <el-form class="small-space" :model="ruleForm" :rules="rules" ref="ruleForm"  label-position="right" label-width="100px" style='width: 400px; margin-left:50px;'>
        
        <el-form-item label="渠道手续费" prop="platrate">
          <el-input v-model="ruleForm.platrate"></el-input>
        </el-form-item>
        <el-form-item label="浮动手续费" prop="floatinrate">
          <el-input v-model="ruleForm.floatinrate"></el-input>
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
import { Platbase, EditPlatbase } from "@/api/user.js";
export default {
  data() {
    var checkplatrate = (rule, value, callback) => {
        if (!Number.parseFloat(value)) {
          callback(new Error("请输入数字值"));
        } 
        else {
           if (value >= 1||value<0.1) {
              callback(new Error("渠道手续费不能大于1或不能小于0.1"));
            } else {
              callback();
            }
        }
    };
    var checkfloatinrate = (rule, value, callback) => {
        if (Number.parseFloat(value)<0) {
           callback(new Error("浮动手续费不能小于0"));
        } 
        else {
           callback();
        }
    };
    return {
      list: null,
      total: null,
      listLoading: true,
      listQuery: {
        code: ""
      },
      ruleForm: {
        platrate: "",
        floatinrate: ""
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
        platrate: [{ validator: checkplatrate, trigger: "blur" }],
        floatinrate: [{ validator: checkfloatinrate, trigger: "blur" }],
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
      this.fetchData();
    },
    handleCreate() {
      this.resetTemp();
      this.dialogStatus = "create";
      this.dialogFormVisible = true;
    },
    handleUpdate(row) {
      this.ruleForm = Object.assign({}, row);
      this.dialogStatus = "update";
      this.dialogFormVisible = true;
    },
    resetTemp() {
      this.ruleForm = {
        roleid: 0,
        rolecode: "",
        rolename: ""
      };
    },
    fetchData() {
      this.listLoading = true;
      Platbase("get", this.listQuery).then(response => {
        this.list = response.data;
        this.listLoading = false;
      });
    },
    update(formName) {
      this.$refs[formName].validate(valid => {
        if (valid) {
          EditPlatbase("put", this.ruleForm)
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
