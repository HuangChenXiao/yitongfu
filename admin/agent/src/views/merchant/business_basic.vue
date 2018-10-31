<template>
  <div class="app-container calendar-list-container">
    <div class="filter-container">
      <el-input @keyup.enter.native="handleFilter" style="width: 200px;" class="filter-item" placeholder="商户名称" v-model="listQuery.code">
      </el-input>
      <el-select v-model="listQuery.merchantid" placeholder="请选择商家名称" class="filter-item">
        <el-option :value="0" label="全部">
        </el-option>
        <el-option v-for="item in merchantlist" :key="item.id" :label="item.name" :value="item.id">
        </el-option>
      </el-select>
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
          <span>{{scope.row.agname}}</span>
        </template>
      </el-table-column>
      <el-table-column label="支付宝账号" align="center">
        <template scope="scope">
          <span>{{scope.row.alipayaccount}}</span>
        </template>
      </el-table-column>
      <el-table-column label="商户简称">
        <template scope="scope">
          <span class="link-type" @click="handleUpdate(scope.row)">{{scope.row.shortName}}</span>
        </template>
      </el-table-column>
      <el-table-column label="状态" align="center">
        <template scope="scope">
          <a href="javascript:;" @click="tagStatus(scope.row)">
            <el-tag :type="scope.row.status==1?'success':''">{{scope.row.status|statusFilter}}</el-tag>
          </a>
        </template>
      </el-table-column>
      <el-table-column label="联系人" align="center">
        <template scope="scope">
          <span>{{scope.row.contactMan}}</span>
        </template>
      </el-table-column>
      <!-- <el-table-column label="商户城市"  align="center">
        <template scope="scope">
          <span>{{scope.row.city}}</span>
        </template>
      </el-table-column> -->
      <el-table-column label="联系手机号" align="center">
        <template scope="scope">
          <span>{{scope.row.mobilePhone}}</span>
        </template>
      </el-table-column>
      <!-- <el-table-column label="余额" align="center">
        <template scope="scope">
          <span style="color:#F8C62E">{{scope.row.balance>0?scope.row.balance:0}}</span>
        </template>
      </el-table-column> -->

      <el-table-column label="appId" align="center">
        <template scope="scope">
          <span>{{scope.row.appId}}</span>
        </template>
      </el-table-column>

      <el-table-column label="appSecret" align="center">
        <template scope="scope">
          <span>{{scope.row.appSecret}}</span>
        </template>
      </el-table-column>
      <!-- <el-table-column label="微信手续费（%）" align="center">
        <template scope="scope">
          <span>{{scope.row.wechatratio}}</span>
        </template>
      </el-table-column>
      <el-table-column label="支付宝手续费（%）" align="center">
        <template scope="scope">
          <span>{{scope.row.alipayratio}}</span>
        </template>
      </el-table-column>
      <el-table-column label="银联手续费（%）" align="center">
        <template scope="scope">
          <span>{{scope.row.unionratio}}</span>
        </template>
      </el-table-column>

      <el-table-column label="提现费用" align="center">
        <template scope="scope">
          <span>{{scope.row.depositratio}}</span>
        </template>
      </el-table-column>

      <el-table-column label="最小金额" align="center">
        <template scope="scope">
          <span>{{scope.row.minAmount}}</span>
        </template>
      </el-table-column>

      <el-table-column label="最大金额" align="center">
        <template scope="scope">
          <span>{{scope.row.maxAmount}}</span>
        </template>
      </el-table-column>
      <el-table-column label="营业开始时间" align="center">
        <template scope="scope">
          <a href="javascript:;">
            {{scope.row.enabletime}}
          </a>
        </template>
      </el-table-column>
      <el-table-column label="营业结束时间" align="center">
        <template scope="scope">
          {{scope.row.disabletime}}
        </template>
      </el-table-column> -->
      <!-- <el-table-column label="审核状态" align="center">
        <template scope="scope">
          <el-tag :type="scope.row.isaudit==true?'success':''">{{scope.row.isaudit==true?"已审核":"未审核"}}</el-tag>
        </template>
      </el-table-column> -->
      <el-table-column label="操作" width="180">
        <template scope="scope">
          <el-button @click.prevent="handleUpdate(scope.row)" type="text" size="small">编辑</el-button>
          <!-- <el-button @click.prevent="unionBalance(scope.row)" type="primary" size="small">合并余额</el-button> -->
          <el-button @click.prevent="quicklogon(scope.row)" type="text" size="small">一键登录</el-button>
        </template>
      </el-table-column>
    </el-table>

    <div v-show="!listLoading" class="pagination-container">
      <el-pagination @size-change="handleSizeChange" @current-change="handleCurrentChange" :current-page.sync="listQuery.page" :page-sizes="[10,20,30, 50]" :page-size="listQuery.pagesize" layout="total, sizes, prev, pager, next, jumper" :total="total">
      </el-pagination>
    </div>

    <el-dialog :title="textMap[dialogStatus]" :visible.sync="dialogFormVisible" :close-on-click-modal="false">
      <el-form class="small-space" :model="ruleForm" :rules="rules" ref="ruleForm" label-position="left" label-width="150px" style='width: 620px; margin-left:50px;'>

        <!-- <el-form-item label="大商户" prop="merchantid">
          <el-select filterable v-model="ruleForm.merchantid" placeholder="请选择大商户" style="width:100%">
            <el-option v-for="item in merchantlist" :key="item.id" :label="item.name" :value="item.id">
            </el-option>
          </el-select>
        </el-form-item> -->
        <el-form-item label="支付宝账号" prop="alipayaccount">
          <el-select filterable v-model="ruleForm.alipayaccount" placeholder="请选择支付宝账号" style="width:100%">
            <el-option v-for="item in pay_passlist" :key="item.memo" :label="item.memo" :value="item.memo">
            </el-option>
          </el-select>
        </el-form-item>
        <el-form-item label="登录名称" prop="merchantName">
          <el-input v-model="ruleForm.merchantName"></el-input>
        </el-form-item>
        <el-form-item label="密码" prop="password">
          <el-input v-model="ruleForm.password" type="password"></el-input>
        </el-form-item>
        <el-form-item label="商户简称" prop="shortName">
          <el-input v-model="ruleForm.shortName"></el-input>
        </el-form-item>
        <el-form-item label="微信手续费（%）" prop="wechatratio">
          <el-input v-model="ruleForm.wechatratio"></el-input>
        </el-form-item>
        <el-form-item label="支付宝手续费（%）" prop="alipayratio">
          <el-input v-model="ruleForm.alipayratio"></el-input>
        </el-form-item>
        <el-form-item label="提现费用" prop="depositratio">
          <el-input v-model="ruleForm.depositratio"></el-input>
        </el-form-item>

        <el-form-item label="收款最小金额" prop="minAmount">
          <el-input v-model.number="ruleForm.minAmount"></el-input>
        </el-form-item>
        <el-form-item label="收款最大金额" prop="maxAmount">
          <el-input v-model.number="ruleForm.maxAmount"></el-input>
        </el-form-item>
        <el-form-item label="用户状态">
          <el-radio-group v-model="ruleForm.status">
            <el-radio :label="1">启用</el-radio>
            <el-radio :label="0">关闭</el-radio>
          </el-radio-group>
        </el-form-item>

        <el-form-item label="联系人" prop="contactMan">
          <el-input v-model="ruleForm.contactMan"></el-input>
        </el-form-item>
        <el-form-item label="联系人手机" prop="mobilePhone">
          <el-input v-model="ruleForm.mobilePhone"></el-input>
        </el-form-item>
        <el-form-item label="营业开始时间" prop="enabletime">
          <el-time-select style="width:100%" v-model="ruleForm.enabletime" :picker-options="{
              start: '06:00',
              step: '00:10',
              end: '23:59'
            }" placeholder="选择时间">
          </el-time-select>
        </el-form-item>
        <el-form-item label="营业结束时间" prop="disabletime">
          <el-time-select style="width:100%" v-model="ruleForm.disabletime" :picker-options="{
              start: '06:00',
              step: '00:10',
              end: '23:59'
            }" placeholder="选择时间">
          </el-time-select>
        </el-form-item>
        <el-form-item label="备注">
          <el-input v-model="ruleForm.remark" type="textarea" :rows="2"></el-input>
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
import {
  AgentMerchant,
  AgentBusinessBasinc,
  editbusinessbasic,
  BusinessPass,
  UnionBalance,
  PayPassList
} from '@/api/merchant.js'
import { merchantcity, businesscategory, bankcode } from '@/api/basic.js'
import { merchBaseDataRegister } from '@/api/java_user.js'
import {
  validateidentitycard,
  validatePhone,
  validatenumber
} from '@/utils/validate.js'
import md5 from 'js-md5'
import { mapGetters } from 'vuex'

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
    const isvalidateidentitycard = (rule, value, callback) => {
      if (!validateidentitycard(value)) {
        callback(new Error('请输入正确身份证号'))
      } else {
        callback()
      }
    }
    const isvalidatePhone = (rule, value, callback) => {
      if (!validatePhone(value)) {
        callback(new Error('请输入正确手机号'))
      } else {
        callback()
      }
    }
    const date_change = (rule, value, callback) => {
      if (!value) {
        callback(new Error('请选择时间'))
      } else {
        callback()
      }
    }
    var min_amount_change = (rule, value, callback) => {
      if (!validatenumber(value)) {
        callback(new Error('请输入正确金额'))
      } else {
        if (value >= this.ruleForm.maxAmount && this.ruleForm.maxAmount) {
          callback(new Error('收款最小金额不能大于等于收款最大金额'))
        } else {
          callback()
        }
      }
    }
    var max_amount_change = (rule, value, callback) => {
      if (!validatenumber(value)) {
        callback(new Error('请输入正确金额'))
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
      listLoading: true,
      listQuery: {
        page: 1,
        pagesize: 10,
        code: ''
      },
      business_pass: null,
      merchantlist: [],
      pay_passlist:[],
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
        merchantid: [
          { required: true, message: '请选择大商户', trigger: 'change' }
        ],
        businesspasstype: [{ validator: bus_pas_change, trigger: 'change' }],
        code: [{ required: true, message: '请输入登录名称', trigger: 'blur' }],
        password: [{ validator: validatePass, trigger: 'blur' }],
        merchantName: [
          { required: true, message: '请输入商户全称', trigger: 'blur' }
        ],
        shortName: [
          { required: true, message: '请输入商户简称', trigger: 'blur' }
        ],
        city: [
          { required: true, message: '请选择商户城市', trigger: 'change' }
        ],
        merchantAddress: [
          { required: true, message: '请输入商户地址', trigger: 'blur' }
        ],
        servicePhone: [
          { required: true, message: '请输入客服电话', trigger: 'blur' }
        ],
        // orgCode: [{ required: true, message: "请输入用户名称", trigger: "blur" }],
        merchantType: [
          { required: true, message: '请输入商户类型', trigger: 'blur' }
        ],
        category: [
          { required: true, message: '请选择经营类目', trigger: 'change' }
        ],
        corpmanName: [
          { required: true, message: '请输入法人姓名', trigger: 'blur' }
        ],
        corpmanId: [{ validator: isvalidateidentitycard, trigger: 'blur' }],
        corpmanPhone: [
          { required: true, message: '请输入法人联系电话', trigger: 'blur' }
        ],
        corpmanMobile: [{ validator: isvalidatePhone, trigger: 'blur' }],
        corpmanEmail: [
          { required: true, message: '请输入法人邮箱', trigger: 'blur' }
        ],
        bankCode: [
          { required: true, message: '请选择银行代码', trigger: 'change' }
        ],
        bankName: [
          { required: true, message: '请输入开户行全称', trigger: 'blur' }
        ],
        bankaccountNo: [
          { required: true, message: '请输入开户行账号', trigger: 'blur' }
        ],
        bankaccountName: [
          { required: true, message: '请输入开户户名', trigger: 'blur' }
        ],
        autoCus: [{ required: true, message: '选择自动提现', trigger: 'blur' }],
        addrType: [
          { required: true, message: '请选择地址类型', trigger: 'change' }
        ],
        contactType: [
          { required: true, message: '请选择联系人类型', trigger: 'change' }
        ],
        mcc: [{ required: true, message: '请选择行业编码', trigger: 'change' }],
        licenseType: [
          { required: true, message: '请选择营业执照类型', trigger: 'change' }
        ],
        contactMan: [
          { required: true, message: '请输入联系人', trigger: 'blur' }
        ],
        telNo: [
          { required: true, message: '请输入联系人电话', trigger: 'blur' }
        ],
        mobilePhone: [
          { required: true, message: '请输入联系人手机', trigger: 'blur' }
        ],
        email: [
          { required: true, message: '请输入联系人邮箱', trigger: 'blur' }
        ],
        licenseBeginDate: [{ validator: date_change, trigger: 'blur' }],
        licenseEndDate: [{ validator: date_change, trigger: 'blur' }],
        licenseRange: [
          { required: true, message: '请输入营业执照经营范围', trigger: 'blur' }
        ],
        cashcode: [
          { required: true, message: '请输入收银员编号', trigger: 'blur' }
        ],
        cashpwd: [
          { required: true, message: '请输入收银员密码', trigger: 'blur' }
        ],
        enabletime: [
          { required: true, message: '请选择营业开始时间', trigger: 'change' }
        ],
        disabletime: [
          { required: true, message: '请选择营业结束时间', trigger: 'change' }
        ],
        ratio: [{ validator: ratio_change, trigger: 'blur' }],
        minAmount: [{ validator: min_amount_change, trigger: 'blur' }],
        maxAmount: [{ validator: max_amount_change, trigger: 'blur' }],
        depositratio: [{ validator: ratio_change, trigger: 'blur' }],
        agentid: [{ validator: agentid_change, trigger: 'change' }]
      }
    }
  },
  computed: {
    ...mapGetters(['userid'])
  },
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
    this.getbasic()
    PayPassList('get', {
      page: 1,
      pagesize: 9999,
      keyword: '',
      status: 2
    }).then(response => {
      this.pay_passlist = response.data
    })
  },
  methods: {
    resetTemp() {
      this.ruleForm = {
        id: '', //商户id
        code: '', //登录名称
        password: '', //密码
        status: 1, //商户状态
        merchantid: 1, //商家id
        merchantName: '', //商户全称
        shortName: '', //商户简称
        handleType: '0', //0新增 1修改
        city: '', //商户城市
        merchantAddress: '', //商户地址
        servicePhone: '', //客服电话
        orgCode: '', //组织机构代码
        merchantType: '01', //商户类型
        category: '', //经营类目代码
        corpmanName: '', //法人姓名
        corpmanId: '', //法人身份证
        corpmanPhone: '', //法人联系电话
        corpmanMobile: '', //法人联系手机
        corpmanEmail: '', //法人邮箱
        bankCode: '', //银行代码
        bankName: '', //开户行全称
        bankaccountNo: '', //开户行账号
        bankaccountName: '', //开户户名
        autoCus: '0', //自动提现
        remark: '', //备注
        licenseNo: '', //营业执照
        taxRegisterNo: '', //税务登记证号码
        agencyId: '', //代理商编号
        appId: '', //appId
        appSecret: '', //appSecret
        addrType: 'REGISTERED_ADDRESS', //REGISTERED_ADDRESS:注册地址 BUSINESS_ADDRESS:经营地址
        contactType: 'OTHER', //OTHER:其他 LEGAL_PERSON:法人 CONTROLLER:实际控制人 AGENT:代理人
        mcc: '', //
        licenseType: 'NATIONAL_LEGAL_MERGE',
        contactMan: '',
        telNo: '',
        mobilePhone: '',
        email: '',
        licenseBeginDate: '',
        licenseEndDate: '',
        licenseRange: '',
        cashcode: '',
        cashpwd: '',
        ratio: '',
        minAmount: '',
        maxAmount: '',
        disabletime: '', //营业结束时间
        enabletime: '', //营业开始时间
        depositratio: '',
        agentid: this.userid,
        businesspasstype: null,
        wechatratio: '',
        alipayratio: '',
        unionratio: '',
        alipayaccount:null
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
      row.handleType = '1'
      row.password = ''
      row.merchantid = row.merchantid.toString()
      this.ruleForm = Object.assign({}, row)
      this.dialogStatus = 'update'
      this.dialogFormVisible = true
    },
    unionBalance(row) {
      this.$confirm('此操作合并余额, 是否继续?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).then(() => {
        this.ruleForm = Object.assign({}, row)
        UnionBalance('get', { businessid: this.ruleForm.id }).then(response => {
          this.dialogFormVisible = false
          this.fetchData()
          this.$notify({
            title: '成功',
            message: '合并成功',
            type: 'success',
            duration: 2000
          })
        })
      })
    },
    quicklogon(row) {
      if (row.bus_domain) {
        window.open(
          'http://' +
            row.bus_domain +
            '/#/quicklogon/' +
            row.merchantName +
            '/' +
            row.password +
            '/' +
            md5(row.merchantName + row.password + 'buyunchina')
        )
      } else {
        window.open(
          'http://admin.az818.com/business/#/quicklogon/' +
            row.merchantName +
            '/' +
            row.password +
            '/' +
            md5(row.merchantName + row.password + 'buyunchina')
        )
      }
      // console.log("http://localhost:9620/#/quicklogon/"+row.merchantName+"/"+row.password+"/"+md5(row.merchantName+row.password+"buyunchina"))
      // window.open("http://localhost:9620/#/quicklogon/"+row.merchantName+"/"+row.password+"/"+md5(row.merchantName+row.password+"buyunchina"))
    },
    audit(row) {
      this.$confirm('此操作向渠道添加商户名称, 是否继续?', '提示', {
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
        merchBaseDataRegister('get', {
          data: formdata
        }).then(response => {
          if (response.data.returnState) {
            var resdata = response.data.data
            if (resdata.status_code == 1) {
              if (resdata.data.merchantNo) {
                this.ruleForm.appId = resdata.data.merchantNo
              }
              if (resdata.data.merchantKey) {
                this.ruleForm.appSecret = resdata.data.merchantKey
              }
              this.ruleForm.isaudit = true
              this.ruleForm.handleType = '1'
              editbusinessbasic('put', this.ruleForm).then(response => {})
              this.$notify({
                title: '成功',
                message: '审核成功',
                type: 'success',
                duration: 0
              })
            } else {
              this.$message.error(resdata.message)
            }
          } else {
            this.$message.error(response.data.message)
          }
        })
      })
    },
    fetchData() {
      this.listLoading = true
      AgentBusinessBasinc('get', this.listQuery).then(response => {
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
      AgentMerchant('get').then(response => {
        this.merchantlist = response.data
      })
      merchantcity('get').then(response => {
        this.citylist = response.data
      })
      businesscategory('get').then(response => {
        this.business_category = response.data
      })
      bankcode('get').then(response => {
        this.bank_code = response.data
      })
      BusinessPass('get').then(response => {
        this.business_pass = response.data
      })
    },
    update(formName) {
      this.$refs[formName].validate(valid => {
        if (valid) {
          editbusinessbasic('put', this.ruleForm).then(response => {
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
          editbusinessbasic('post', this.ruleForm).then(response => {
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
