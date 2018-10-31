<template>
  <div class="app-container calendar-list-container">
    <div class="filter-container">
      <el-input @keyup.enter.native="handleFilter" style="width: 200px;" class="filter-item" placeholder="代理商名称" v-model="listQuery.code">
      </el-input>

      <el-button class="filter-item" type="primary" icon="search" @click="handleFilter">搜索</el-button>
      <el-button class="filter-item" style="margin-left: 10px;" @click="handleCreate" type="primary" icon="edit">添加</el-button>
    </div>
    <el-table :data="list" v-loading.body="listLoading" element-loading-text="拼命加载中" border fit highlight-current-row>
      <el-table-column align="center" label='序号' width="95">
        <template scope="scope">
          {{scope.$index+1}}
        </template>
      </el-table-column>
      <el-table-column label="用户名称">
        <template scope="scope">
          <span class="link-type" @click="handleUpdate(scope.row)">{{scope.row.name}}</span>
        </template>
      </el-table-column>
      <el-table-column label="成本(%)" align="center" width="120">
        <template scope="scope">
          <span>{{scope.row.ratio}}</span>
        </template>
      </el-table-column>
      <el-table-column label="佣金" align="center" width="100">
        <template scope="scope">
          <span>{{scope.row.commission}}</span>
        </template>
      </el-table-column>
      <el-table-column label="公司名称" align="center">
        <template scope="scope">
          <span>{{scope.row.corporate_name}}</span>
        </template>
      </el-table-column>
      <!-- <el-table-column label="代理级别" align="center" width="100">
        <template scope="scope">
          {{scope.row.agency_level}}
        </template>
      </el-table-column> -->
      <el-table-column label="注册时间" align="center" width="200">
        <template scope="scope">
          {{scope.row.addtime|dateFilter}}
        </template>
      </el-table-column>
     
      <el-table-column label="状态" align="center" width="100">
        <template scope="scope">
          <el-tag :type="scope.row.user_status==1?'success':''">{{scope.row.user_status|statusFilter}}</el-tag>
        </template>
      </el-table-column>
      <el-table-column fixed="right" label="操作" width="150">
        <template scope="scope">
          <el-button @click.prevent="handleUpdate(scope.row)" type="text" size="small">编辑</el-button>
          <el-button @click.prevent="quicklogon(scope.row)" type="text" size="small">一键登录</el-button>
          <!-- <el-button @click.prevent="handleDelete(scope.row)" type="text" size="small">删除</el-button> -->
        </template>
      </el-table-column>
    </el-table>

    <div v-show="!listLoading" class="pagination-container">
      <el-pagination @size-change="handleSizeChange" @current-change="handleCurrentChange" :current-page.sync="listQuery.page" :page-sizes="[10,20,30, 50]" :page-size="listQuery.pagesize" layout="total, sizes, prev, pager, next, jumper" :total="total">
      </el-pagination>
    </div>

    <el-dialog :title="textMap[dialogStatus]" :visible.sync="dialogFormVisible" :close-on-click-modal="false">
      <el-form :model="ruleForm" :rules="rules" ref="ruleForm" label-position="left" label-width="150px" style='width: 620px; margin-left:50px;'>
        <el-form-item label="用户名称" prop="name">
          <el-input v-model="ruleForm.name"></el-input>
        </el-form-item>
        <el-form-item label="密码" prop="password">
          <el-input type="password" v-model="ruleForm.password"></el-input>
        </el-form-item>
        <el-form-item label="成本(%)" prop="ratio">
          <el-input v-model.number="ruleForm.ratio"></el-input>
        </el-form-item>
        <!-- <el-form-item label="佣金" prop="commission">
          <el-input v-model.number="ruleForm.commission"></el-input>
        </el-form-item> -->
        <!-- <el-form-item label="开户名">
          <el-input v-model="ruleForm.account_name"></el-input>
        </el-form-item>
        <el-form-item label="卡号">
          <el-input v-model="ruleForm.card_number"></el-input>
        </el-form-item>
        <el-form-item label="开户行">
          <el-input v-model="ruleForm.bank_accounts"></el-input>
        </el-form-item>
        <el-form-item label="开户地址">
          <el-input v-model="ruleForm.opening_address"></el-input>
        </el-form-item>

        <el-form-item label="开户网点">
          <el-input v-model="ruleForm.opening_point"></el-input>
        </el-form-item>
         <el-form-item label="代理级别" prop="agency_level">
          <el-select v-model="ruleForm.agency_level" placeholder="请选择代理级别" style="width:100%">
            <el-option label="全国代理" :value="0"></el-option>
            <el-option label="省级代理" :value="1"></el-option>
            <el-option label="市级代理" :value="2"></el-option>
            <el-option label="区级代理" :value="3"></el-option>
          </el-select>
        </el-form-item>

         <el-form-item label="直营身份">
          <el-checkbox-group v-model="ruleForm.direct_identity">
            <el-checkbox label="直营" key="true"></el-checkbox>
          </el-checkbox-group>
        </el-form-item>

        <el-form-item label="代理商权限" prop="agent_rights">
          <el-checkbox-group v-model="ruleForm.agent_rights">
            <el-checkbox label="0" key="0">特约</el-checkbox>
            <el-checkbox label="1" key="1">修改费率</el-checkbox>
            <el-checkbox label="2" key="2">快速实名认证</el-checkbox>
          </el-checkbox-group>
        </el-form-item>

        <el-form-item label="代理金额">
          <el-input v-model="ruleForm.agency_amount" type="number"></el-input>
        </el-form-item> -->

        <el-form-item label="代理到期时间">
          <el-date-picker style="width:100%" v-model="ruleForm.agency_expiration_date" type="date" placeholder="选择日期时间">
          </el-date-picker>
        </el-form-item>
        <el-form-item label="用户状态">
          <el-radio-group v-model="ruleForm.user_status">
            <el-radio :label="1">启用</el-radio>
            <el-radio :label="2">待审核</el-radio>
            <el-radio :label="3">锁定</el-radio>
            <el-radio :label="4">暂停开号</el-radio>
            <el-radio :label="0">废弃</el-radio>
          </el-radio-group>
        </el-form-item>
        <el-form-item label="备注说明">
          <el-input v-model="ruleForm.remarks"></el-input>
        </el-form-item>
        <el-form-item label="公司名称" prop="corporate_name">
          <el-input v-model="ruleForm.corporate_name"></el-input>
        </el-form-item>
        <!-- <el-form-item label="所属行业" prop="industry_owned">
          <el-input v-model="ruleForm.industry_owned"></el-input>
        </el-form-item>
        <el-form-item label="主营业务" prop="main_business">
          <el-input v-model="ruleForm.main_business"></el-input>
        </el-form-item>
        <el-form-item label="公司人数" prop="company_number">
          <el-input v-model="ruleForm.company_number" type="number"></el-input>
        </el-form-item>
        <el-form-item label="年营业额" prop="annual_turnover">
          <el-input v-model="ruleForm.annual_turnover" type="number"></el-input>
        </el-form-item> -->
        <el-form-item label="联系人" prop="contacts">
          <el-input v-model="ruleForm.contacts"></el-input>
        </el-form-item>

        <el-form-item label="联系电话" prop="contact_number">
          <el-input v-model="ruleForm.contact_number"></el-input>
        </el-form-item>
        <el-form-item label="手机号" prop="mobile_phone">
          <el-input v-model="ruleForm.mobile_phone"></el-input>
        </el-form-item>
        <el-form-item label="qq号">
          <el-input v-model="ruleForm.qq_number"></el-input>
        </el-form-item>
        <el-form-item label="电子邮箱" prop="mail_box">
          <el-input v-model="ruleForm.mail_box"></el-input>
        </el-form-item>
        <!-- <el-form-item label="合同文件" prop="proof_document">
          <el-upload :action="baseurl+'Upload'" list-type="picture-card" :on-success="handleListAvatarSuccess" :on-remove="handleRemove" :before-upload="beforeAvatarUpload" :file-list="getproof_document" :headers="{'Authorization':agToken}">
            <i class="el-icon-plus"></i>
          </el-upload>
          <el-dialog :visible.sync="dialogVisible" size="tiny">
            <img width="100%" :src="ruleForm.proof_document" alt="">
          </el-dialog>
        </el-form-item>
        <el-form-item label="公司Logo" prop="company_logo">
          <el-upload class="avatar-uploader" :action="baseurl+'Upload'" :show-file-list="false" :on-success="handleAvatarSuccess" :before-upload="beforeAvatarUpload" :headers="{'Authorization':agToken}">
            <img v-if="ruleForm.company_logo" :src="ruleForm.company_logo" class="avatar">
            <i v-else class="el-icon-plus avatar-uploader-icon"></i>
          </el-upload>
        </el-form-item>
        <el-form-item label="域名" prop="domain">
          <el-input v-model="ruleForm.domain" placeholder="例如：www.agent.com"></el-input>
        </el-form-item>
        <el-form-item label="商户域名" prop="bus_domain">
          <el-input v-model="ruleForm.bus_domain" placeholder="例如：www.bus.com"></el-input>
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
import { agent, editagent } from '@/api/user.js'
import { getToken } from '@/utils/auth'
import md5 from 'js-md5'

export default {
  data() {
    var validatePass = (rule, value, callback) => {
      if (value === '' && this.dialogStatus == 'create') {
        callback(new Error('请输入密码'))
      } else {
        callback()
      }
    }
    var validatePass2 = (rule, value, callback) => {
      if (value === '') {
        callback(new Error('请再次输入密码'))
      } else if (value !== this.ruleForm.password) {
        callback(new Error('两次输入密码不一致!'))
      } else {
        callback()
      }
    }
    var ckh_proof_document = (rule, value, callback) => {
      if (value.length <= 0) {
        callback(new Error('请上传合同等必要的证明文件的扫描'))
      } else {
        callback()
      }
    }
    var ckh_agent_rights = (rule, value, callback) => {
      if (value.length <= 0) {
        callback(new Error('请选择代理商权限'))
      } else {
        callback()
      }
    }
    var checkratio = (rule, value, callback) => {
      if (!Number.parseFloat(value)) {
        callback(new Error('请输入数字值'))
      } else {
        if (value >= 10 || value <= 0.28) {
          callback(new Error('成本(%)不能大于10或不能小于0.28'))
        } else {
          callback()
        }
      }
    }
    return {
      agToken: getToken(),
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
      ruleForm: {
        id: '', // 用户id
        name: '', //用户名称
        password: '', //密码
        account_name: '', //开户名
        card_number: '', //卡号
        bank_accounts: '', //开户行
        opening_address: '', //开户地址
        opening_point: '', //开户网点
        agency_level: '', //代理级别(0全国代理 1省级代理 2市级代理 3区级代理)
        direct_identity: '', //直营身份
        agent_rights: [], //代理商权限(0特约 1修改费率 2快速实名认证)
        agency_amount: '', //代理金额
        agency_expiration_date: '', //代理到期时间
        user_status: 1, //用户状态(1启用 2待审核 3锁定 4暂停开号 0废弃)
        remarks: '', //备注说明
        corporate_name: '', //公司名称
        province: '', //省
        city: '', //市
        area: '', //区
        industry_owned: '', //所属行业
        main_business: '', //主营业务
        company_number: '', //公司人数
        annual_turnover: '', //年营业额
        contacts: '', //联系人
        contact_number: '', //联系电话
        mobile_phone: '', //手机
        qq_number: '', //QQ号码
        mail_box: '', //电子邮箱
        proof_document: [], //上传合同等必要的证明文件的扫描件
        company_logo: '', //上传公司logo
        is_alipay: '', //拥有支付宝开通权限
        is_jd: '', //拥有京东开通权限
        is_t0: '', //T0权限
        commission: '', //代理商佣金
        domain:null,
        bus_domain:null
      },
      dialogFormVisible: false,
      dialogStatus: '',
      textMap: {
        update: '编辑',
        create: '创建'
      },
      dialogImageUrl: '',
      dialogVisible: false,
      dialogPvVisible: false,
      pvData: [],
      rules: {
        name: [{ required: true, message: '请输入用户名称', trigger: 'blur' }],
        password: [{ validator: validatePass, trigger: 'blur' }],
        agent_rights: [{ validator: ckh_agent_rights, trigger: 'blur' }],
        // agency_expiration_date: [
        //   { required: true, message: "请输入代理到期时间", trigger: "change" }
        // ],
        industry_owned: [
          { required: true, message: '请输入公司名称', trigger: 'blur' }
        ],
        main_business: [
          { required: true, message: '请输入公司名称', trigger: 'blur' }
        ],
        // company_number: [
        //   { required: true, message: "请输入公司人数", trigger: "blur" }
        // ],
        // annual_turnover: [
        //   { required: true, message: "请输入年营业额", trigger: "blur" }
        // ],
        contacts: [
          { required: true, message: '请输入联系人', trigger: 'blur' }
        ],
        contact_number: [
          { required: true, message: '请输入联系电话', trigger: 'blur' }
        ],
        mobile_phone: [
          { required: true, message: '请输入手机', trigger: 'blur' }
        ],
        mail_box: [
          { required: true, message: '请输入电子邮箱', trigger: 'blur' }
        ],
        // proof_document: [{ required: true, message: "请上传合同等必要的证明文件的扫描", trigger: "change" }],
        proof_document: [{ validator: ckh_proof_document, trigger: 'blur' }],
        company_logo: [
          { required: true, message: '请输入上传公司logo', trigger: 'change' }
        ],
        ratio: [{ validator: checkratio, trigger: 'blur' }]
      }
    }
  },
  computed: {
    getproof_document() {
      console.log(typeof this.ruleForm.proof_document)
      if (this.ruleForm.proof_document.length > 0) {
        var imglist = []
        if (typeof this.ruleForm.proof_document == 'object') {
          for (var i = 0; i < this.ruleForm.proof_document.length; i++) {
            imglist.push({ url: this.ruleForm.proof_document[i] })
          }
          return imglist
        } else {
          var list = this.ruleForm.proof_document.split(',')
          for (var i = 0; i < list.length; i++) {
            imglist.push({ url: list[i] })
          }
          return imglist
        }
      }
      return []
    }
  },
  filters: {
    statusFilter(status) {
      const statusMap = {
        1: '启用',
        2: '待审核',
        3: '锁定',
        4: '暂停开号',
        0: '废弃'
      }
      return statusMap[status]
    },
    metaroleFilter(meta) {
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
    },
    dateFilter(value) {
      if (value) {
        return value.replace('T', ' ')
      }
    }
  },
  created() {
    // console.log(this.cfg.formatDate(new Date()))
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
      if (this.ruleForm.agent_rights.length > 0) {
        var list = this.ruleForm.agent_rights.split(',')
        var imglist = []
        for (var i = 0; i < list.length; i++) {
          imglist.push(list[i])
        }
        this.ruleForm.agent_rights = imglist
      } else {
        this.ruleForm.agent_rights = []
      }
      this.ruleForm.password = ''
      this.dialogStatus = 'update'
      this.dialogFormVisible = true
    },
    handleDelete(row) {
      this.$confirm('此操作删除该路由, 是否继续?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).then(() => {
        this.ruleForm = Object.assign({}, row)
        agent('delete', { id: this.ruleForm.id }).then(response => {
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
    },
    fetchData() {
      this.listLoading = true
      agent('get', this.listQuery).then(response => {
        this.list = response.data
        this.total = response.total

        this.listLoading = false
      })
    },
    resetTemp() {
      this.ruleForm = {
        id: '', // 用户id
        name: '', //用户名称
        password: '', //密码
        account_name: '', //开户名
        card_number: '', //卡号
        bank_accounts: '', //开户行
        opening_address: '', //开户地址
        opening_point: '', //开户网点
        agency_level: '', //代理级别(0全国代理 1省级代理 2市级代理 3区级代理)
        direct_identity: '', //直营身份
        agent_rights: [], //代理商权限(0特约 1修改费率 2快速实名认证)
        agency_amount: '', //代理金额
        agency_expiration_date: '', //代理到期时间
        user_status: 1, //用户状态(1启用 2待审核 3锁定 4暂停开号 0废弃)
        remarks: '', //备注说明
        corporate_name: '', //公司名称
        province: '', //省
        city: '', //市
        area: '', //区
        industry_owned: '', //所属行业
        main_business: '', //主营业务
        company_number: '', //公司人数
        annual_turnover: '', //年营业额
        contacts: '', //联系人
        contact_number: '', //联系电话
        mobile_phone: '', //手机
        qq_number: '', //QQ号码
        mail_box: '', //电子邮箱
        proof_document: [], //上传合同等必要的证明文件的扫描件
        company_logo: '', //上传公司logo
        is_alipay: '', //拥有支付宝开通权限
        is_jd: '', //拥有京东开通权限
        is_t0: '' //T0权限
      }
      this.rolelist = []
    },
    update(formName) {
      this.$refs[formName].validate(valid => {
        if (valid) {
          this.ruleForm.agent_rights = this.ruleForm.agent_rights.toString()
          this.ruleForm.proof_document = this.ruleForm.proof_document.toString()
          editagent('put', this.ruleForm).then(response => {
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
          this.ruleForm.agent_rights = this.ruleForm.agent_rights.toString()
          this.ruleForm.proof_document = this.ruleForm.proof_document.toString()
          editagent('post', this.ruleForm).then(response => {
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
    handleRemove(file, fileList) {
      if (fileList.length > 0) {
        var imglist = []
        for (var i = 0; i < fileList.length; i++) {
          imglist.push(fileList[i].url)
        }
        this.ruleForm.proof_document = imglist
      } else {
        this.ruleForm.proof_document = []
      }
    },
    handlePictureCardPreview(file) {
      this.ruleForm.proof_document = file.response.url
      this.dialogVisible = true
    },
    handleListAvatarSuccess(res, file) {
      if (typeof this.ruleForm.proof_document == 'object') {
        this.ruleForm.proof_document.push(res.url)
      } else {
        if (this.ruleForm.proof_document) {
          var imglist = this.ruleForm.proof_document.split(',')
          imglist.push(res.url)
          this.ruleForm.proof_document = imglist
        }
      }
    },
    handleAvatarSuccess(res, file) {
      this.ruleForm.company_logo = res.url
    },
    beforeAvatarUpload(file) {
      const isJPG = file.type === 'image/jpeg'
      const isLt2M = file.size / 1024 / 1024 < 2

      if (!isJPG) {
        this.$message.error('上传头像图片只能是 JPG 格式!')
      }
      if (!isLt2M) {
        this.$message.error('上传头像图片大小不能超过 2MB!')
      }
      return isJPG && isLt2M
    },
    quicklogon(row) {
      if(row.domain){
        window.open("http://"+row.domain+"/#/quicklogon/"+row.name+"/"+row.password+"/"+md5(row.name+row.password+"buyunchina"))
      }
      else{
        window.open("http://admin.az818.com/agent/#/quicklogon/"+row.name+"/"+row.password+"/"+md5(row.name+row.password+"buyunchina"))
      }
      // window.open("http://localhost:9628/#/quicklogon/"+row.name+"/"+row.password+"/"+md5(row.name+row.password+"buyunchina"))
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

<style>
.avatar-uploader .el-upload {
  border: 1px dashed #c0ccda;
  border-radius: 6px;
  cursor: pointer;
  position: relative;
  overflow: hidden;
}
.avatar-uploader .el-upload:hover {
  border-color: #409eff;
}
.avatar-uploader-icon {
  font-size: 28px;
  color: #8c939d;
  width: 148px;
  height: 148px;
  line-height: 148px;
  text-align: center;
}
.avatar {
  width: 148px;
  height: 148px;
  display: block;
}
</style>
