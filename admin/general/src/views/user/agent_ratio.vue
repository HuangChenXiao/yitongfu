<template>
    <div class="app-container calendar-list-container">
        <div class="filter-container">
            <el-input @keyup.enter.native="handleFilter" style="width: 200px;" class="filter-item" placeholder="代理商" v-model="listQuery.code">
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
            <el-table-column label="代理商" align="center">
                <template scope="scope">
                    <span>{{scope.row.name}}</span>
                </template>
            </el-table-column>
            <el-table-column label="支付通道" align="center">
                <template scope="scope">
                    <span>{{scope.row.businesspasstype|PayMethod}}</span>
                </template>
            </el-table-column>
            <el-table-column label="手续费（%）" align="center">
                <template scope="scope">
                    <span>{{scope.row.ratio}}</span>
                </template>
            </el-table-column>
            <el-table-column label="操作" width="180">
                <template scope="scope">
                    <el-button @click.prevent="handleUpdate(scope.row)" type="text" size="small">编辑</el-button>
                </template>
            </el-table-column>
        </el-table>

        <div v-show="!listLoading" class="pagination-container">
            <el-pagination @size-change="handleSizeChange" @current-change="handleCurrentChange" :current-page.sync="listQuery.page" :page-sizes="[10,20,30, 50]" :page-size="listQuery.pagesize" layout="total, sizes, prev, pager, next, jumper" :total="total">
            </el-pagination>
        </div>

        <el-dialog :title="textMap[dialogStatus]" :visible.sync="dialogFormVisible" :close-on-click-modal="false">
            <el-form class="small-space" :model="ruleForm" :rules="rules" ref="ruleForm" label-position="left" label-width="150px" style='width: 620px; margin-left:50px;'>
                <el-form-item label="代理商名称" prop="agid">
                    <el-select v-model="ruleForm.agid" filterable  placeholder="请选择代理商" style="width:100%">
                        <el-option v-for="item in agentlist" :key="item.id" :label="item.name" :value="item.id">
                        </el-option>
                    </el-select>
                </el-form-item>
                <el-form-item label="支付通道" prop="businesspasstype">
                    <el-select class="filter-item" style="width:100%" v-model="ruleForm.businesspasstype" placeholder="支付通道">
                        <el-option v-for="item in  business_pass" :key="item.type" :label="item.memo" :value="item.type" v-if="item.enable">
                        </el-option>
                    </el-select>
                </el-form-item>
                <el-form-item label="手续费（%）" prop="ratio">
                    <el-input v-model="ruleForm.ratio"></el-input>
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
import { AgentRatio, EditAgentRatio, agent, BusinessPass } from '@/api/user.js'
import {
  validateidentitycard,
  validatePhone,
  validatenumber
} from '@/utils/validate.js'
import md5 from 'js-md5'

export default {
  data() {
    var ratio_change = (rule, value, callback) => {
      if (!validatenumber(value)) {
        callback(new Error('请输入正确手续费'))
      } else {
        callback()
      }
    }
    var agentid_change = (rule, value, callback) => {
      if (!value) {
        callback(new Error('请选择代理商'))
      } else {
        callback()
      }
    }
    var bus_pas_change = (rule, value, callback) => {
      if (!value) {
        callback(new Error('请选择支付通道'))
      } else {
        callback()
      }
    }
    return {
      list: null,
      total: null,
      business_type: null,
      listLoading: true,
      business_pass: null,
      agentlist: [],
      listQuery: {
        page: 1,
        pagesize: this.basepagesize,
        code: '',
        merchantid: 0
      },
      merchantlist: [],
      citylist: [], //城市列表
      business_category: [], //经营类目
      bank_code: [], //银行代码
      ruleForm: '',
      dialogFormVisible: false,
      dialogStatus: '',
      textMap: {
        update: '编辑',
        create: '创建'
      },
      dialogPvVisible: false,
      rules: {
        ratio: [{ validator: ratio_change, trigger: 'blur' }],
        agid: [{ validator: agentid_change, trigger: 'change' }],
        businesspasstype: [{ validator: bus_pas_change, trigger: 'change' }]
      }
    }
  },
  computed: {},
  filters: {
    statusFilter(status) {
      const statusMap = {
        1: '启用',
        0: '关闭'
      }
      return statusMap[status]
    },
    dateFilter(value) {
      if (value) {
        return value.replace('T', ' ')
      }
    }
  },
  created() {
    this.fetchData()
    this.getagent()
  },
  methods: {
    resetTemp() {
      this.ruleForm = {
        id:null,
        agid:null,
        businesspasstype:null,
        ratio:null,
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
      row.password = ''
      row.agentid = parseInt(row.agentid)
      this.ruleForm = Object.assign({}, row)
      this.dialogStatus = 'update'
      this.dialogFormVisible = true
    },
    getagent() {
      this.listLoading = true
      agent('get', { page: 1, pagesize: 9999, code: '' }).then(response => {
        this.agentlist = response.data
      })
    },
    audit(row) {
      this.$confirm('此操作获取收银员信息, 是否继续?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).then(() => {
        this.ruleForm = Object.assign({}, row)
        getKey('get', {
          data: {
            username: row.cashcode,
            password: row.cashpwd
          }
        }).then(res => {
          if (res.data.data.statue == '1') {
            let data = res.data.data.data
            this.ruleForm.cashier_num = data.cashier_num
            this.ruleForm.merchant_num = data.merchant_num
            this.ruleForm.cash_token = data.token

            setCashid('put', this.ruleForm).then(res => {
              this.$notify({
                title: '成功',
                message: '审核成功',
                type: 'success',
                duration: 2000
              })
            })
          } else {
            this.$message({
              message: '审核失败',
              type: 'warning'
            })
          }
        })
      })
    },
    fetchData() {
      this.listLoading = true
      AgentRatio('get', this.listQuery).then(response => {
        this.list = response.data
        this.total = response.total

        this.listLoading = false
      })
      BusinessPass('get').then(response => {
        this.business_pass = response.data
      })
    },
    update(formName) {
      this.$refs[formName].validate(valid => {
        if (valid) {
          EditAgentRatio('put', this.ruleForm).then(response => {
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
          EditAgentRatio('post', this.ruleForm).then(response => {
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
    }
  },
  watch: {
    dialogFormVisible(val, oldValue) {
      if (!val) {
        this.$refs['ruleForm'].resetFields()
      }
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
