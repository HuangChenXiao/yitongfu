<template>
    <div class="app-container calendar-list-container">
        <div class="filter-container">
            <el-input @keyup.enter.native="handleFilter" style="width: 200px;" class="filter-item" placeholder="模糊搜索" v-model="listQuery.keyword">
            </el-input>
            <el-select clearable class="filter-item" style="width: 200px" v-model="listQuery.status" placeholder="状态">
                <el-option :key="2" label="全部" :value="2">
                </el-option>
                <el-option :key="0" label="禁用" :value="0">
                </el-option>
                <el-option :key="1" label="启用" :value="1">
                </el-option>
            </el-select>
            <el-button class="filter-item" type="primary" icon="search" @click="handleFilter">搜索</el-button>
            <el-button class="filter-item" style="margin-left: 10px;" @click="handleCreate" type="primary" icon="edit">添加</el-button>
        </div>
        <el-table :data="list" v-loading.body="listLoading" element-loading-text="拼命加载中" border fit highlight-current-row>
            <el-table-column align="center" label='序号' width="95">
                <template scope="scope">
                    {{scope.$index+1}}
                </template>
            </el-table-column>
            <el-table-column label="支付宝appid" align="center">
                <template scope="scope">
                    <span>{{scope.row.alipayappid}}</span>
                </template>
            </el-table-column>
            <el-table-column label="金额" align="center">
                <template scope="scope">
                    <span>{{scope.row.dayamount}}</span>
                </template>
            </el-table-column>
            <el-table-column align="center" label="备注" width="100">
                <template scope="scope">
                    <span>{{scope.row.memo}}</span>
                </template>
            </el-table-column>
            <el-table-column label="操作" align="center" width="100">
                <template scope="scope">
                    <el-button @click.prevent="handleUpdate(scope.row)" type="text">编辑</el-button>
                </template>
            </el-table-column>
        </el-table>

        <div v-show="!listLoading" class="pagination-container">
            <el-pagination @size-change="handleSizeChange" @current-change="handleCurrentChange" :current-page.sync="listQuery.page" :page-sizes="[10,20,30, 50]" :page-size="listQuery.pagesize" layout="total, sizes, prev, pager, next, jumper" :total="total">
            </el-pagination>
        </div>

        <el-dialog :title="textMap[dialogStatus]" :visible.sync="dialogFormVisible">
            <el-form class="small-space" :model="ruleForm" :rules="rules" ref="ruleForm" label-position="right" label-width="100px" style='width: 400px; margin-left:50px;'>
                <el-form-item label="支付宝appid" prop="alipayappid">
                    <el-input v-model="ruleForm.alipayappid"></el-input>
                </el-form-item>
                <el-form-item label="私钥" prop="rsaprivate">
                    <el-input type="textarea" :rows="5" v-model="ruleForm.rsaprivate"></el-input>
                </el-form-item>
                <el-form-item label="公钥" prop="rsapublic">
                    <el-input type="textarea" :rows="5" v-model="ruleForm.rsapublic"></el-input>
                </el-form-item>
                <el-form-item label="金额" prop="dayamount">
                    <el-input v-model="ruleForm.dayamount"></el-input>
                </el-form-item>
                <el-form-item label="备注" prop="memo">
                    <el-input v-model="ruleForm.memo"></el-input>
                </el-form-item>
                <el-form-item label="状态" prop="enable">
                    <el-radio :label="false" v-model="ruleForm.enable">禁用</el-radio>
                    <el-radio :label="true" v-model="ruleForm.enable">启用</el-radio>
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
import { validatenumber } from '@/utils/validate.js'
import { PayPassList } from '@/api/alipay.js'
export default {
  data() {
    var checknumber = (rule, value, callback) => {
      if (!validatenumber(value)) {
        callback(new Error('请输入数值类型'))
      } else {
        callback()
      }
    }
    return {
      list: null,
      total: null,
      selectlist: null,
      listLoading: true,
      listQuery: {
        page: 1,
        pagesize: this.basepagesize,
        keyword: '',
        status: 2
      },
      ruleForm: {},
      dialogFormVisible: false,
      dialogStatus: '',
      textMap: {
        update: '编辑',
        create: '创建'
      },
      dialogPvVisible: false,
      pvData: [],
      rules: {
        alipayappid: [
          { required: true, message: '请输入支付宝appid', trigger: 'blur' }
        ],
        rsaprivate: [
          { required: true, message: '请输入私钥', trigger: 'blur' }
        ],
        rsapublic: [{ required: true, message: '请输入公钥', trigger: 'blur' }],
        dayamount: [{ validator: checknumber, trigger: 'blur' }]
      }
    }
  },
  filters: {
    statusFilter(status) {
      const statusMap = {
        1: '启用',
        2: '禁用'
      }
      return statusMap[status]
    }
  },
  created() {
    this.fetchData()
  },
  methods: {
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
      this.ruleForm.password = ''
      this.dialogStatus = 'update'
      this.dialogFormVisible = true
    },
    resetTemp() {
      this.ruleForm = {
        id: 0,
        alipayappid: null,
        rsaprivate: null,
        rsapublic: null,
        dayamount: null,
        memo: null,
        enable: true
      }
    },
    fetchData() {
      this.listLoading = true
      PayPassList('get', this.listQuery).then(response => {
        this.list = response.data
        this.total = response.data.length > 0 ? response.data.length : 0

        this.listLoading = false
      })
    },
    update(formName) {
      this.$refs[formName].validate(valid => {
        if (valid) {
          PayPassList('post', this.ruleForm).then(response => {
            this.dialogFormVisible = false
            this.fetchData()
            this.$notify({
              title: '成功',
              message: '更新成功',
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
          PayPassList('post', this.ruleForm).then(response => {
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
</style>
