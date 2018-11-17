<template>
    <div class="app-container calendar-list-container">
        <div class="filter-container">
            <el-input @keyup.enter.native="handleFilter" style="width: 200px;" class="filter-item" placeholder="商户编码" v-model="listQuery.code">
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
            <el-table-column label="商户名称" align="center">
                <template scope="scope">
                    <span>{{scope.row.sh_businessname}}</span>
                </template>
            </el-table-column>
            <el-table-column label="持卡人手机" align="center">
                <template scope="scope">
                    <span>{{scope.row.mobileNo}}</span>
                </template>
            </el-table-column>
            <el-table-column label="银行卡号" align="center">
                <template scope="scope">
                    <span>{{scope.row.bankaccountNo}}</span>
                </template>
            </el-table-column>
            <el-table-column label="持卡人姓名" align="center">
                <template scope="scope">
                    <span>{{scope.row.name}}</span>
                </template>
            </el-table-column>
            <el-table-column label="开户省份" align="center">
                <template scope="scope">
                    <span>{{scope.row.province}}</span>
                </template>
            </el-table-column>
            <el-table-column label="开户城市" align="center">
                <template scope="scope">
                    <span>{{scope.row.city}}</span>
                </template>
            </el-table-column>
            <el-table-column label="开户支行" align="center">
                <template scope="scope">
                    <span>{{scope.row.bankaddress}}</span>
                </template>
            </el-table-column>
            <!-- <el-table-column label="审核状态"  align="center">
        <template scope="scope">
          <el-tag :type="scope.row.isaudit==true?'success':''">{{scope.row.isaudit==true?"已审核":"未审核"}}</el-tag>
        </template>
      </el-table-column> -->
            <el-table-column fixed="right" label="操作" width="100">
                <template scope="scope">
                    <el-button @click.prevent="handleUpdate(scope.row)" type="text" size="small">编辑</el-button>
                    <!-- <el-button @click.prevent="audit(scope.row)" type="primary" size="small" :disabled="scope.row.isaudit==true?true:false">审核</el-button> -->
                </template>
            </el-table-column>
        </el-table>

        <div v-show="!listLoading" class="pagination-container">
            <el-pagination @size-change="handleSizeChange" @current-change="handleCurrentChange" :current-page.sync="listQuery.page" :page-sizes="[10,20,30, 50]" :page-size="listQuery.pagesize" layout="total, sizes, prev, pager, next, jumper" :total="total">
            </el-pagination>
        </div>

        <el-dialog :title="textMap[dialogStatus]" :visible.sync="dialogFormVisible" :close-on-click-modal="false">
            <el-form class="small-space" :model="ruleForm" :rules="rules" ref="ruleForm" label-position="left" label-width="150px" style='width: 620px; margin-left:50px;'>
                <el-form-item label="商户名称" prop="sh_businessid">
                    <el-select v-model="ruleForm.sh_businessid" filterable placeholder="请选择商户名称" style="width:100%">
                        <el-option v-for="item in business_basic" :key="item.id" :label="item.sh_businessname" :value="item.id">
                        </el-option>
                    </el-select>
                </el-form-item>
                <el-form-item label="持卡人手机号" prop="mobileNo">
                    <el-input v-model="ruleForm.mobileNo"></el-input>
                </el-form-item>
                <!-- <el-form-item label="操作类型" prop="handleType">
          <el-radio-group v-model="ruleForm.handleType">
          <el-radio label="0">新增</el-radio>
          <el-radio label="1">删除</el-radio>
        </el-radio-group>
        </el-form-item> -->
                <el-form-item label="银行代码" prop="bankCode">
                    <el-select v-model="ruleForm.bankCode" filterable placeholder="请选择银行代码" style="width:100%">
                        <el-option v-for="item in bank_code" :key="item.code" :label="item.explain" :value="item.code">
                        </el-option>
                    </el-select>
                </el-form-item>
                <!-- <el-form-item label="账户属性" prop="bankaccProp">
          <el-radio-group v-model="ruleForm.bankaccProp">
          <el-radio label="0">私人</el-radio>
          <el-radio label="1">公司</el-radio>
        </el-radio-group>
        </el-form-item> -->
                <el-form-item label="持卡人姓名" prop="name">
                    <el-input v-model="ruleForm.name"></el-input>
                </el-form-item>
                <!-- <el-form-item label="银行卡类型">
          <el-radio-group v-model="ruleForm.bankaccountType">
          <el-radio label="1">借记卡</el-radio>
          <el-radio label="2">贷记卡</el-radio>
          <el-radio label="3">存折</el-radio>
        </el-radio-group>
        </el-form-item> -->
                <el-form-item label="银行卡号">
                    <el-input v-model="ruleForm.bankaccountNo"></el-input>
                </el-form-item>
                <el-form-item label="证件类型">
                    <el-select v-model="ruleForm.certCode" filterable placeholder="请选择证件类型" style="width:100%">
                        <el-option label="身份证" value="1"></el-option>
                        <el-option label="护照" value="2"></el-option>
                        <el-option label="军官证" value="3"></el-option>
                        <el-option label="回乡证" value="4"></el-option>
                        <el-option label="台胞证" value="5"></el-option>
                        <el-option label="港澳通行证" value="6"></el-option>
                        <el-option label="国际海员证" value="7"></el-option>
                        <el-option label="外国人永久居住证" value="8"></el-option>
                        <el-option label="其它" value="9"></el-option>
                    </el-select>
                </el-form-item>
                <el-form-item label="证件号码" prop="certNo">
                    <el-input v-model="ruleForm.certNo"></el-input>
                </el-form-item>
                <el-form-item label="开户省份" prop="province">
                    <el-input v-model="ruleForm.province"></el-input>
                </el-form-item>
                <el-form-item label="开户城市" prop="city">
                    <el-input v-model="ruleForm.city"></el-input>
                </el-form-item>
                <el-form-item label="开户支行" prop="bankaddress">
                    <el-input v-model="ruleForm.bankaddress"></el-input>
                </el-form-item>
                <!-- <el-form-item label="银联号">
          <el-input v-model="ruleForm.bankbranchNo"></el-input>
        </el-form-item> -->
                <!-- <el-form-item label="默认账户" prop="defaultAcc">
          <el-radio-group v-model="ruleForm.defaultAcc">
          <el-radio label="0">否</el-radio>
          <el-radio label="1">是</el-radio>
        </el-radio-group>
        </el-form-item> -->
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
import { ShBankCard, EditShBankCard } from '@/api/shmanager.js'
import { SHBusinessAppinfoList } from '@/api/user.js'
import { bankcode } from '@/api/basic.js'

export default {
  data() {
    var validatePass = (rule, value, callback) => {
      //密码
      if (value === '' && this.dialogStatus == 'create') {
        callback(new Error('请输入密码'))
      } else {
        callback()
      }
    }
    var businessid_change = (rule, value, callback) => {
      if (!value) {
        callback(new Error('请选择商户名称'))
      } else {
        callback()
      }
    }
    return {
      list: null,
      total: null,
      listLoading: true,
      listQuery: {
        page: 1,
        pagesize: this.basepagesize,
        code: '',
        merchantid: ''
      },
      business_category: [], //经营类目
      bank_code: [], //银行代码
      business_basic: [], //商户列表
      ruleForm: '',
      dialogFormVisible: false,
      dialogStatus: '',
      textMap: {
        update: '编辑',
        create: '创建'
      },
      dialogPvVisible: false,
      rules: {
        sh_businessid: [{ validator: businessid_change, trigger: 'change' }],
        mobileNo: [
          { required: true, message: '请输入持卡人手机号', trigger: 'blur' }
        ],
        handleType: [
          { required: true, message: '请选择操作类型', trigger: 'change' }
        ],
        bankCode: [
          { required: true, message: '请选择银行代码', trigger: 'change' }
        ],
        bankaccProp: [
          { required: true, message: '请选择账户属性', trigger: 'change' }
        ],
        name: [
          { required: true, message: '请输入持卡人姓名', trigger: 'blur' }
        ],
        bankaccountType: [
          { required: true, message: '请选择银行卡类型', trigger: 'change' }
        ],
        certCode: [
          { required: true, message: '请选择办卡证件类型', trigger: 'change' }
        ],
        certNo: [
          { required: true, message: '请输入证件号码', trigger: 'blur' }
        ],
        bankbranchNo: [
          { required: true, message: '请输入联行号', trigger: 'blur' }
        ],
        defaultAcc: [
          { required: true, message: '请选择默认账户', trigger: 'change' }
        ],
        province: [
          { required: true, message: '请输入开户省份', trigger: 'blur' }
        ],
        city: [{ required: true, message: '请输入开户城市', trigger: 'blur' }],
        bankaddress: [
          { required: true, message: '请输入开户支行', trigger: 'blur' }
        ]
      }
    }
  },
  computed: {},
  filters: {
    statusFilter(status) {
      const statusMap = {
        1: '启用',
        2: '关闭',
        0: '违规'
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
    this.getbasic()
  },
  methods: {
    resetTemp() {
      this.ruleForm = {
        id: '', //id
        sh_businessid: '', //商户编号
        mobileNo: '', //持卡人手机号
        MobileNo2: '', //持卡人手机号2
        handleType: '0', //操作类型
        bankCode: '', //银行代码
        bankaccProp: '0', //账户属性
        name: '', //持卡人姓名
        bankaccountType: '1', //银行卡类型 1-借记卡（账户属性为0-私人时，只能上送1）；2-贷记卡；3-存折
        bankaccountNo: '', //银行卡号
        certCode: '', //办卡证件类型
        certNo: '', //证件号码
        bankbranchNo: '', //联行号
        defaultAcc: '1', //默认账户
        bankEightCode: '',
        province: null,
        city: null,
        bankaddress: null
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
      this.ruleForm.MobileNo2 = this.ruleForm.mobileNo
      this.dialogStatus = 'create'
      this.dialogFormVisible = true
    },
    handleUpdate(row) {
      row.businessid = parseInt(row.businessid)
      this.ruleForm = Object.assign({}, row)
      this.dialogStatus = 'update'
      this.dialogFormVisible = true
    },
    audit(row) {
      this.$confirm('此操作向渠道添加银行卡, 是否继续?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).then(() => {
        this.ruleForm = Object.assign({}, row)
        var formdata = Object.assign({}, row)
        this.$message({
          showClose: true,
          message: '请求发送成功，请等待后台返回状态后在进行操作！',
          type: 'success',
          duration: 5000
        })
        merchBankCardRegister('get', {
          data: formdata
        }).then(response => {
          if (response.data.returnState) {
            var resdata = response.data.data
            if (resdata.status_code == 1) {
              this.ruleForm.isaudit = true
              EditShBankCard('put', this.ruleForm).then(response => {})
              this.$notify({
                title: '成功',
                message: '审核成功',
                type: 'success',
                duration: 0
              })
            } else {
              if (resdata.message.indexOf('重复') >= 0) {
                //如果重复直接修改状态
                this.ruleForm.isaudit = true
                EditShBankCard('put', this.ruleForm).then(response => {
                  this.$notify({
                    title: '成功',
                    message: '审核成功',
                    type: 'success',
                    duration: 3000
                  })
                  this.fetchData()
                })
              } else {
                this.$message.error(resdata.message)
              }
            }
          } else {
            this.$message.error(response.data.message)
          }
        })
      })
    },
    fetchData() {
      this.listLoading = true
      ShBankCard('get', this.listQuery).then(response => {
        this.list = response.data
        this.total = response.total

        this.listLoading = false
      })
      // console.log(bData);
      // this.list = bData.router;
      // this.total = bData.router.length;
      // this.listLoading = false;
    },
    getbasic() {
      bankcode('get').then(response => {
        this.bank_code = response.data
      })
      SHBusinessAppinfoList('get', {
        page: 1,
        pagesize: 999,
        status: 1
      }).then(response => {
        this.business_basic = response.data
      })
    },
    update(formName) {
      this.$refs[formName].validate(valid => {
        if (valid) {
          EditShBankCard('put', this.ruleForm).then(response => {
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
          EditShBankCard('post', this.ruleForm).then(response => {
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
