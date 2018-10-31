<template>
  <div class="app-container calendar-list-container">
    <div class="filter-container">
      <el-input @keyup.enter.native="handleFilter" style="width: 200px;" class="filter-item" placeholder="路由名称" v-model="listQuery.code">
      </el-input>

      <el-button class="filter-item" type="primary" icon="search" @click="handleFilter">搜索</el-button>
      <el-button class="filter-item" style="margin-left: 10px;" @click="handleCreate" type="primary" icon="edit">添加</el-button>
    </div>
    <el-table :data="list" v-loading.body="listLoading" element-loading-text="拼命加载中" border fit highlight-current-row> 
      <el-table-column align="center" label='序号' width="95">
        <template scope="scope">
          {{scope.$index}}
        </template>
      </el-table-column>
      <el-table-column  label="路由名称">
        <template scope="scope">
          <span class="link-type" @click="handleUpdate(scope.row)">{{scope.row.name}}</span>
        </template>
      </el-table-column>
      <el-table-column label="路由路径"  align="center">
        <template scope="scope">
          <span>{{scope.row.path}}</span>
        </template>
      </el-table-column>
      <el-table-column label="重定向"  align="center">
        <template scope="scope">
          <span>{{scope.row.redirect}}</span>
        </template>
      </el-table-column>
      <el-table-column label="图标" align="center">
        <template scope="scope">
          {{scope.row.icon}}
        </template>
      </el-table-column>

      <el-table-column label="路由不下拉" align="center">
        <template scope="scope">
          {{scope.row.nodropdown}}
        </template>
      </el-table-column>
      <el-table-column label="是否隐藏" align="center">
        <template scope="scope">
          {{scope.row.hidden}}
        </template>
      </el-table-column>
      <el-table-column label="关联角色" align="center">
        <template scope="scope">
          {{scope.row.meta}}
        </template>
      </el-table-column>
      <el-table-column label="操作" align="center" width="100">
        <template scope="scope">
          <el-button @click.prevent="handleDelete(scope.row)" type="text">删除</el-button>
        </template>
      </el-table-column>
    </el-table>

    <div v-show="!listLoading" class="pagination-container">
      <el-pagination @size-change="handleSizeChange" @current-change="handleCurrentChange" :current-page.sync="listQuery.page"
        :page-sizes="[10,20,30, 50]" :page-size="listQuery.pagesize" layout="total, sizes, prev, pager, next, jumper" :total="total">
      </el-pagination>
    </div>

    <el-dialog :title="textMap[dialogStatus]" :visible.sync="dialogFormVisible">
      <el-form class="small-space" :model="ruleForm" :rules="rules" ref="ruleForm" label-position="left" label-width="90px" style='width: 400px; margin-left:50px;'>
        <el-form-item label="路由名称" prop="name">
          <el-input v-model="ruleForm.name"></el-input>
        </el-form-item>
        <el-form-item label="路由路径" prop="path">
          <el-input v-model="ruleForm.path"></el-input>
        </el-form-item>
        <el-form-item label="重定向" prop="redirect">
          <el-input v-model="ruleForm.redirect"></el-input>
        </el-form-item>
        <el-form-item label="图标" prop="icon">
          <el-input v-model="ruleForm.icon"></el-input>
        </el-form-item>
        <el-form-item label="路由不下拉">
          <el-select v-model="ruleForm.nodropdown" placeholder="请选择下拉状态" style="width:100%">
            <el-option label="不下拉" value="true"></el-option>
            <el-option label="下拉" value="false"></el-option>
          </el-select>
        </el-form-item>
        <el-form-item label="是否隐藏">
          <el-select v-model="ruleForm.hidden" placeholder="请选择是否隐藏" style="width:100%">
            <el-option label="显示" value="false"></el-option>
            <el-option label="隐藏" value="true"></el-option>
          </el-select>
        </el-form-item>
        <el-form-item label="排序" prop="sort">
          <el-input v-model="ruleForm.sort"></el-input>
        </el-form-item>
        <el-form-item label="可访问角色">
          <el-select v-model="role" placeholder="请选择用户角色"  style="width:79%">
          <el-option
            v-for="item in getrolelist"
            :key="item.rolecode"
            :label="item.rolename"
            :value="item.rolecode">
          </el-option>
        </el-select>
          <el-button @click.prevent="addRole()">添加 </el-button>
        </el-form-item>
        <el-form-item label="关联角色">
          <!-- {{ruleForm.meta.role}} -->
            <el-tag
              v-for="tag in rolelist"
              :key="tag"
              closable
              type="success"
              @close="handleClose(tag)">
              {{tag}}
            </el-tag>
        </el-form-item>
      </el-form>
      

      <div slot="footer" class="dialog-footer">
        <el-button @click="dialogFormVisible = false">取 消</el-button>
        <el-button v-if="dialogStatus=='create'" type="primary" @click="create('ruleForm')">确 定 </el-button>
        <el-button v-else type="primary" @click="update('ruleForm')">确 定</el-button>
      </div>
    </el-dialog>
  </div>
</template>

<script>

import { router,editrouter,role } from "@/api/authority.js";

export default {
  data() {
    return {
      list: null,
      total: null,
      listLoading: true,
      listQuery: {
        page: 1,
        pagesize: this.basepagesize,
        code: "",
      },
      role: "",
      rolelist: [],
      getrolelist: [],
      ruleForm: {
        id: "0",
        name: "",
        path: "",
        redirect: "noredirect",
        icon: "tubiao",
        nodropdown: "true",
        meta: "",
        sort: "0",
        hidden: "false",
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
        name: [{ required: true, message: "请输入路由名称", trigger: "blur" }],
        path: [{ required: true, message: "请输入路由路径", trigger: "blur" }],
        redirect: [{ required: true, message: "请输入路由重定向", trigger: "blur" }],
        icon: [{ required: true, message: "请输入路由图标", trigger: "blur" }]
      }
    };
  },
  computed: {
    getmeta() {
      var strtext = "";
      for (var i = 0; i < this.rolelist.length; i++) {
        if (strtext) {
          strtext += "," + this.rolelist[i];
        } else {
          strtext += this.rolelist[i];
        }
      }
      return strtext;
    }
  },
  filters: {
    statusFilter(status) {
      const statusMap = {
        1: "启用",
        2: "禁用"
      };
      return statusMap[status];
    },
    metaroleFilter(meta) {
      var _html = "";
      if (meta.length <= 0 && meta.role.length <= 0) {
        return "";
      }
      for (var i = 0; i < meta.role.length; i++) {
        if (!_html) {
          _html += meta.role[i];
        } else {
          _html += "，" + meta.role[i];
        }
      }
      return _html;
    }
  },
  created() {
    this.fetchData();
    this.getRoleList();
  },
  methods: {
    handleClose(tag) {
      this.rolelist.splice(this.rolelist.indexOf(tag), 1);
    },
    addRole() {
      if (this.role) {
        if (this.rolelist.indexOf(this.role) >= 0) {
          this.$message({
            message: "角色已经存在",
            type: "warning"
          });
          return;
        }
        this.rolelist.push(this.role);
      }
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
    handleCreate() {
      this.resetTemp();
      this.dialogStatus = "create";
      this.dialogFormVisible = true;
    },
    handleUpdate(row) {
      this.ruleForm = Object.assign({}, row);
      this.ruleForm.nodropdown = row.nodropdown.toString();
      this.ruleForm.hidden = row.hidden.toString();
      let list = row.meta ? row.meta.split(",") : [];
      this.rolelist = [];
      for (var i = 0; i < list.length; i++) {
        this.rolelist.push(list[i]);
      }
      this.dialogStatus = "update";
      this.dialogFormVisible = true;
    },
    handleDelete(row) {
      this.$confirm("此操作删除该路由, 是否继续?", "提示", {
        confirmButtonText: "确定",
        cancelButtonText: "取消",
        type: "warning"
      })
        .then(() => {
          this.ruleForm = Object.assign({}, row);
          router('delete',{"id":this.ruleForm.id}).then(response => {
            this.dialogFormVisible = false;
            this.fetchData();
            this.$notify({
              title: "成功",
              message: "删除成功",
              type: "success",
              duration: 2000
            });
          });
        })
    },
    fetchData() {
      this.listLoading = true;
      router("get",this.listQuery).then(response => {
        this.list = response.data;
        this.total =response.total;

        this.listLoading = false;
      });
      // console.log(bData);
      // this.list = bData.router;
      // this.total = bData.router.length;
      // this.listLoading = false;
    },
    getRoleList() {
      this.listLoading = true;
      role('get').then(response => {
        this.getrolelist = response.data;
      });
      // console.log(bData);
      // this.list = bData.router;
      // this.total = bData.router.length;
      // this.listLoading = false;
    },
    resetTemp() {
      this.ruleForm = {
        id: "0",
        name: "",
        path: "",
        redirect: "noredirect",
        icon: "tubiao",
        nodropdown: "true",
        hidden: "false",
        meta: "",
        sort: "0",
      };
      this.rolelist = [];
    },
    update(formName) {
      this.$refs[formName].validate(valid => {
        if (valid) {
          this.ruleForm.meta = this.getmeta;
          editrouter('put',this.ruleForm).then(response => {
            this.dialogFormVisible = false;
            this.fetchData();
            this.$notify({
              title: "成功",
              message: "修改成功",
              type: "success",
              duration: 2000
            });
          });
        } else {
          console.log("error submit!!");
          return false;
        }
      });
    },
    create(formName) {
      this.$refs[formName].validate(valid => {
        if (valid) {
          this.ruleForm.meta = this.getmeta;
          editrouter('post',this.ruleForm).then(response => {
            this.dialogFormVisible = false;
            this.fetchData();
            this.$notify({
              title: "成功",
              message: "新增成功",
              type: "success",
              duration: 2000
            });
          });
        } else {
          console.log("error submit!!");
          return false;
        }
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
.el-tag + .el-tag {
  margin-left: 10px;
}
</style>
