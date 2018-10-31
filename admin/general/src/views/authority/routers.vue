<template>
  <div class="app-container calendar-list-container">
    <div class="filter-container">
      <el-select clearable class="filter-item" style="width: 200px" v-model="listQuery.code" placeholder="主路由名称">
        <el-option v-for="item in  main_router_list" :key="item.name" :label="item.name" :value="item.name">
        </el-option>
      </el-select>
      <el-button class="filter-item" type="primary" icon="search" @click="handleFilter">搜索</el-button>
      <el-button class="filter-item" style="margin-left: 10px;" @click="handleCreate" type="primary" icon="edit">添加</el-button>
    </div>
    <el-table :data="list" v-loading.body="listLoading" element-loading-text="拼命加载中" border fit highlight-current-row @selection-change="handleSelectionChange">
      <el-table-column align="center" label='序号' width="95">
        <template scope="scope">
          {{scope.$index}}
        </template>
      </el-table-column>
      <el-table-column label="关联主路由">
        <template scope="scope">
          <span>{{scope.row.router_name}}</span>
        </template>
      </el-table-column>
      <el-table-column label="路由名称">
        <template scope="scope">
          <span class="link-type" @click="handleUpdate(scope.row)">{{scope.row.name}}</span>
        </template>
      </el-table-column>
      <el-table-column label="路由路径" align="center">
        <template scope="scope">
          <span>{{scope.row.path}}</span>
        </template>
      </el-table-column>
      <el-table-column label="路由组件" align="center">
        <template scope="scope">
          <span>{{scope.row.component}}</span>
        </template>
      </el-table-column>
      <el-table-column label="排序" align="center">
        <template scope="scope">
          <span>{{scope.row.sort}}</span>
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
      <el-table-column label="操作" align="center" width="150">
        <template scope="scope">
          <el-button @click.prevent="handleCopy(scope.row)" type="text">复制</el-button>
          <el-button @click.prevent="handleDelete(scope.row)" type="text">删除</el-button>
        </template>
      </el-table-column>
    </el-table>

    <div v-show="!listLoading" class="pagination-container">
      <el-pagination @size-change="handleSizeChange" @current-change="handleCurrentChange" :current-page.sync="listQuery.page" :page-sizes="[10,20,30, 50]" :page-size="listQuery.pagesize" layout="total, sizes, prev, pager, next, jumper" :total="total">
      </el-pagination>
    </div>

    <el-dialog :title="textMap[dialogStatus]" :visible.sync="dialogFormVisible">
      <el-form class="small-space" :model="ruleForm" :rules="rules" ref="ruleForm" label-position="left" label-width="90px" style='width: 400px; margin-left:50px;'>
        <el-form-item label="主路由">
          <el-select v-model="ruleForm.id" placeholder="请选择对应的主路由" style="width:100%">
            <el-option v-for="item in main_router_list" :key="item.id" :label="item.name" :value="item.id">
            </el-option>
          </el-select>
        </el-form-item>
        <el-form-item label="路由名称" prop="name">
          <el-input v-model="ruleForm.name"></el-input>
        </el-form-item>
        <el-form-item label="路由路径" prop="path">
          <el-input v-model="ruleForm.path"></el-input>
        </el-form-item>
        <el-form-item label="路由组件" prop="component">
          <el-input v-model="ruleForm.component"></el-input>
        </el-form-item>
        <el-form-item label="排序">
          <el-input v-model="ruleForm.sort"></el-input>
        </el-form-item>
        <el-form-item label="是否隐藏">
          <el-select v-model="ruleForm.hidden" placeholder="请选择是否隐藏" style="width:100%">
            <el-option label="显示" value="false"></el-option>
            <el-option label="隐藏" value="true"></el-option>
          </el-select>
        </el-form-item>
        <el-form-item label="可访问角色">
          <el-select v-model="role" placeholder="请选择用户角色" style="width:79%">
            <el-option v-for="item in getrolelist" :key="item.rolecode" :label="item.rolename" :value="item.rolecode">
            </el-option>
          </el-select>
          <el-button @click.prevent="addRole()">添加 </el-button>
        </el-form-item>
        <el-form-item label="关联角色">
          <!-- {{ruleForm.meta.role}} -->
          <el-tag v-for="tag in rolelist" :key="tag" closable type="success" @close="handleClose(tag)">
            {{tag}}
          </el-tag>
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
import { router, routers, editrouters, role } from '@/api/authority.js'

export default {
  data() {
    return {
      list: null,
      total: null,
      listLoading: true,
      listQuery: {
        page: 1,
        pagesize: this.basepagesize,
        code: ''
      },
      role: '',
      rolelist: [],
      getrolelist: [],
      main_router_list: '',
      ruleForm: {
        id: '',
        autoid: '',
        name: '',
        path: '',
        component: '',
        meta: '',
        sort: '0',
        hidden: 'false',
        type: 0
      },
      dialogFormVisible: false,
      dialogStatus: '',
      textMap: {
        update: '编辑',
        create: '创建'
      },
      dialogPvVisible: false,
      pvData: [],
      rules: {
        name: [{ required: true, message: '请输入路由名称', trigger: 'blur' }],
        path: [{ required: true, message: '请输入路由路径', trigger: 'blur' }],
        component: [
          { required: true, message: '请输入路由组件', trigger: 'blur' }
        ]
      }
    }
  },
  computed: {
    getmeta() {
      var strtext = ''
      for (var i = 0; i < this.rolelist.length; i++) {
        if (strtext) {
          strtext += ',' + this.rolelist[i]
        } else {
          strtext += this.rolelist[i]
        }
      }
      return strtext
    }
  },
  filters: {
    statusFilter(status) {
      const statusMap = {
        1: '启用',
        2: '禁用'
      }
      return statusMap[status]
    },
    metaroleFilter(meta) {
      debugger
      var _html = ''
      if (meta.length <= 0 && meta.role.length <= 0) {
        return ''
      }
      for (var i = 0; i < meta.role.length; i++) {
        if (!_html) {
          _html += meta.role[i]
        } else {
          _html += '，' + meta.role[i]
        }
      }
      return _html
    }
  },
  created() {
    this.getselectRoute()
    this.fetchData()
    this.getRoleList()
  },
  methods: {
    handleClose(tag) {
      this.rolelist.splice(this.rolelist.indexOf(tag), 1)
    },
    addRole() {
      if (this.role) {
        if (this.rolelist.indexOf(this.role) >= 0) {
          this.$message({
            message: '角色已经存在',
            type: 'warning'
          })
          return
        }
        this.rolelist.push(this.role)
      }
    },
    handleFilter() {
      this.listQuery.page = 1
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
    handleCreate() {
      this.resetTemp()
      this.dialogStatus = 'create'
      this.dialogFormVisible = true
    },
    handleUpdate(row) {
      this.ruleForm = Object.assign({}, row)
      this.ruleForm.hidden = row.hidden.toString()
      let list = row.meta ? row.meta.split(',') : []
      this.rolelist = []
      for (var i = 0; i < list.length; i++) {
        this.rolelist.push(list[i])
      }
      this.dialogStatus = 'update'
      this.dialogFormVisible = true
    },
    handleDelete(row) {
      this.$confirm('此操作删除该路由, 是否继续?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      })
        .then(() => {
          this.ruleForm = Object.assign({}, row)
          routers('delete', { autoid: this.ruleForm.autoid }).then(response => {
            this.dialogFormVisible = false
            this.fetchData()
            this.$notify({
              title: '成功',
              message: '删除成功',
              type: 'success',
              duration: 2000
            })
          })
        })
        .catch(() => {
          this.$message({
            type: 'info',
            message: '已取消删除'
          })
        })
    },
    fetchData() {
      this.listLoading = true
      routers('get', this.listQuery).then(response => {
        this.list = response.data
        this.total = response.total

        this.listLoading = false
      })
    },
    getRoleList() {
      this.listLoading = true
      role('get').then(response => {
        this.getrolelist = response.data
      })
      // console.log(bData);
      // this.list = bData.router;
      // this.total = bData.router.length;
      // this.listLoading = false;
    },
    getselectRoute() {
      router('get', {
        page: 1,
        pagesize: 999,
        code: ''
      }).then(response => {
        this.main_router_list = response.data
      })
    },
    resetTemp() {
      this.ruleForm = {
        id: '',
        autoid: '',
        name: '',
        path: '',
        meta: '',
        sort: '0',
        hidden: 'false',
        type: 0
      }
      this.rolelist = []
    },
    handleCopy(row) {
      this.ruleForm = Object.assign({}, row)
      editrouters('post', this.ruleForm).then(response => {
        this.fetchData()
        this.$notify({
          title: '成功',
          message: '复制成功',
          type: 'success',
          duration: 2000
        })
      })
    },
    update(formName) {
      this.$refs[formName].validate(valid => {
        if (valid) {
          this.ruleForm.meta = this.getmeta
          editrouters('put', this.ruleForm).then(response => {
            this.dialogFormVisible = false
            this.fetchData()
            this.$notify({
              title: '成功',
              message: '修改成功',
              type: 'success',
              duration: 2000
            })
          })
        } else {
          console.log('error submit!!')
          return false
        }
      })
    },
    create(formName) {
      this.$refs[formName].validate(valid => {
        if (valid) {
          this.ruleForm.meta = this.getmeta
          editrouters('post', this.ruleForm).then(response => {
            this.dialogFormVisible = false
            this.fetchData()
            this.$notify({
              title: '成功',
              message: '新增成功',
              type: 'success',
              duration: 2000
            })
          })
        } else {
          console.log('error submit!!')
          return false
        }
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
.el-tag + .el-tag {
  margin-left: 10px;
}
</style>
