<template>
    <div class="app-container calendar-list-container">
        <div class="filter-container">
            <el-input @keyup.enter.native="handleFilter" style="width: 200px;" class="filter-item" placeholder="名称" v-model="listQuery.keyword">
            </el-input>
            <el-date-picker v-model="listQuery.begindate" type="date" placeholder="开始日期">
            </el-date-picker>
            <el-date-picker v-model="listQuery.enddate" type="date" placeholder="结束日期">
            </el-date-picker>

            <el-button class="filter-item" type="primary" icon="search" @click="handleFilter">搜索</el-button>
            <el-button class="filter-item" style="margin-left: 10px;" @click="handleCreate" type="primary" icon="edit">添加</el-button>
        </div>
        <el-table :data="list" v-loading.body="listLoading" element-loading-text="拼命加载中" border fit highlight-current-row>
            <el-table-column align="center" label='序号' width="95">
                <template scope="scope">
                    {{scope.$index+1}}
                </template>
            </el-table-column>
            <el-table-column label="商户名称" align="center">
                <template scope="scope">
                    <span>{{scope.row.name}}</span>
                </template>
            </el-table-column>
            <el-table-column label="佣金金额" align="center">
                <template scope="scope">
                    <span>{{scope.row.comm_amount}}</span>
                </template>
            </el-table-column>
            <el-table-column label="备注" align="center">
                <template scope="scope">
                    <span>{{scope.row.memo}}</span>
                </template>
            </el-table-column>
            <el-table-column label="类型" align="center">
                <template scope="scope">
                    <span><el-tag :type="scope.row.type==1?'danger':'success'">{{scope.row.type|statusFilter}}</el-tag></span>
                </template>
            </el-table-column>
            <el-table-column align="center" prop="created_at" label="新增人" width="100">
                <template scope="scope">
                    <span>{{scope.row.adduser}}</span>
                </template>
            </el-table-column>
            <el-table-column align="center" prop="created_at" label="新增时间" width="200">
                <template scope="scope">
                    <i class="el-icon-time"></i>
                    <span>{{scope.row.addtime}}</span>
                </template>
            </el-table-column>
        </el-table>

        <div v-show="!listLoading" class="pagination-container">
            <el-pagination @size-change="handleSizeChange" @current-change="handleCurrentChange" :current-page.sync="listQuery.page" :page-sizes="[10,20,30, 50]" :page-size="listQuery.pagesize" layout="total, sizes, prev, pager, next, jumper" :total="total">
            </el-pagination>
        </div>

        <el-dialog :title="textMap[dialogStatus]" :visible.sync="dialogFormVisible">
            <el-form class="small-space" :model="ruleForm" :rules="rules" ref="ruleForm" label-position="right" label-width="100px" style='width: 400px; margin-left:50px;'>
                <el-form-item label="大商户" prop="merchantid">
                    <el-select filterable v-model="ruleForm.merchantid" placeholder="请选择" style="width:100%">
                        <el-option v-for="item in mer_list" :key="item.id" :label="item.name" :value="item.id">
                        </el-option>
                    </el-select>
                </el-form-item>
                <el-form-item label="类型" prop="type">
                    <el-select v-model="ruleForm.type" filterable placeholder="请选择" style="width:100%">
                        <el-option key="1" label="扣款" value="1">
                        </el-option>
                        <el-option key="2" label="充值" value="2">
                        </el-option>
                    </el-select>
                </el-form-item>
                <el-form-item label="商户佣金" prop="comm_amount">
                    <el-input v-model="ruleForm.comm_amount"></el-input>
                </el-form-item>
                <el-form-item label="备注" prop="memo">
                    <el-input v-model="ruleForm.memo"></el-input>
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
import { Mercomm, EditMercomm } from '@/api/mercomm.js'
import { merchant } from '@/api/user.js'
import { validatenumber } from '@/utils/validate.js'
export default {
  data() {
    var number_change = (rule, value, callback) => {
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
        merchantid: 0,
        keyword: '',
        begindate: this.cfg.getCurrentMonthFirst(),
        enddate: this.cfg.getCurrentMonthLast(),
        page: 1,
        pagesize: this.basepagesize
      },
      ruleForm: {},
      mer_list: [],
      dialogFormVisible: false,
      dialogStatus: '',
      textMap: {
        update: '编辑',
        create: '创建'
      },
      dialogPvVisible: false,
      pvData: [],
      rules: {
        comm_amount: [
          { required: true, trigger: 'blur', validator: number_change }
        ]
      }
    }
  },
  filters: {
    statusFilter(status) {
      const statusMap = {
        1: '扣款',
        2: '充值'
      }
      return statusMap[status]
    }
  },
  created() {
    this.fetchData()
    merchant('get', {
      page: 1,
      pagesize: 9999
    }).then(response => {
      this.mer_list = response.data
    })
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
        merchantid: '',
        comm_amount: '',
        memo: '',
        type: '1'
      }
    },
    fetchData() {
      this.listLoading = true
      Mercomm('get', this.listQuery).then(response => {
        this.list = response.data
        this.total = response.data? response.data[0].count : 0

        this.listLoading = false
      })
    },
    update(formName) {
      this.$refs[formName].validate(valid => {
        if (valid) {
          EditMercomm('put', this.ruleForm).then(response => {
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
          EditMercomm('get', this.ruleForm).then(response => {
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
