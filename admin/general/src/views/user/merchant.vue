<template>
  <div class="app-container calendar-list-container">
    <div class="filter-container">
      <el-input @keyup.enter.native="handleFilter" style="width: 200px;" class="filter-item" placeholder="商户名称" v-model="listQuery.code">
      </el-input>

      <el-button class="filter-item" type="primary" icon="search" @click="handleFilter">搜索</el-button>
      <el-button class="filter-item" style="margin-left: 10px;" @click="handleCreate" type="primary" icon="edit">添加</el-button>
      <el-button class="filter-item" style="margin-left: 10px;" @click="handleAudit" type="primary">一键审核</el-button>
    </div>
    <el-table :data="list" v-loading.body="listLoading" element-loading-text="拼命加载中" border fit highlight-current-row @selection-change="handleSelectionChange">
      <el-table-column type="selection" width="55">
      </el-table-column>
      <el-table-column align="center" label='序号' width="95">
        <template scope="scope">
          {{scope.$index}}
        </template>
      </el-table-column>
      <el-table-column label="商户名称">
        <template scope="scope">
          <span class="link-type" @click="handleToRoute(scope.row)">{{scope.row.name}}</span>
        </template>
      </el-table-column>
      <el-table-column label="代理商名称" align="center">
        <template scope="scope">
          <span>{{scope.row.agname}}</span>
        </template>
      </el-table-column>
      <el-table-column label="商户号" align="center">
        <template scope="scope">
          <span>{{scope.row.appid}}</span>
        </template>
      </el-table-column>
      <!-- <el-table-column label="appSecret" align="center">
        <template scope="scope">
          <span>{{scope.row.appsecret}}</span>
        </template>
      </el-table-column> -->

      <el-table-column label="佣金比例%" align="center">
        <template scope="scope">
          <span>{{scope.row.comm_ratio}}</span>
        </template>
      </el-table-column>
      <el-table-column label="佣金金额" align="center">
        <template scope="scope">
          <span>{{scope.row.comm_amount}}</span>
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
      </el-table-column>
      <el-table-column label="状态" align="center">
        <template scope="scope">
          <a href="javascript:;" @click="tagStatus(scope.row)">
            <el-tag :type="scope.row.user_status==1?'success':''">{{scope.row.user_status|statusFilter}}</el-tag>
          </a>
        </template>
      </el-table-column>
      <el-table-column label="操作" width="120">
        <template scope="scope">
          <el-button @click.prevent="handleUpdate(scope.row)" type="text" size="small">编辑</el-button>
          <el-button @click.prevent="handleDelete(scope.row)" type="text" size="small">删除</el-button>
        </template>
      </el-table-column>
    </el-table>

    <div v-show="!listLoading" class="pagination-container">
      <el-pagination @size-change="handleSizeChange" @current-change="handleCurrentChange" :current-page.sync="listQuery.page" :page-sizes="[10,20,30, 50]" :page-size="listQuery.pagesize" layout="total, sizes, prev, pager, next, jumper" :total="total">
      </el-pagination>
    </div>

    <el-dialog :title="textMap[dialogStatus]" :visible.sync="dialogFormVisible" :close-on-click-modal="false">
      <el-form class="small-space" :model="ruleForm" :rules="rules" ref="ruleForm" label-position="left" label-width="150px" style='width: 620px; margin-left:50px;'>
        <el-form-item label="商户号" prop="appid">
          <el-input v-model="ruleForm.appid"></el-input>
        </el-form-item>
        <el-form-item label="用户名称" prop="name">
          <el-input v-model="ruleForm.name"></el-input>
        </el-form-item>
        <el-form-item label="密码" prop="password">
          <el-input type="password" v-model="ruleForm.password"></el-input>
        </el-form-item>
        <el-form-item label="代理商名称" prop="agid">
          <el-select v-model="ruleForm.agid" placeholder="请选择代理商" style="width:100%">
            <el-option v-for="item in agentlist" :key="item.id" :label="item.name" :value="item.id">
            </el-option>
          </el-select>
        </el-form-item>
        <el-form-item label="用户状态">
          <el-radio-group v-model="ruleForm.user_status">
            <el-radio :label="1">启用</el-radio>
            <el-radio :label="2">关闭</el-radio>
            <el-radio :label="0">违规</el-radio>
          </el-radio-group>
        </el-form-item>
        <!-- <el-form-item label="用户权限" prop="user_rights">
          <el-checkbox-group v-model="ruleForm.user_rights">
            <el-checkbox label="收银台" key="true"></el-checkbox>
          </el-checkbox-group>
        </el-form-item> -->
        <el-form-item label="佣金比例%" prop="comm_ratio">
          <el-input v-model="ruleForm.comm_ratio"></el-input>
        </el-form-item>
        <el-form-item label="联系人" prop="contacts">
          <el-input v-model="ruleForm.contacts"></el-input>
        </el-form-item>
        <el-form-item label="联系方式" prop="contact_information">
          <el-input v-model="ruleForm.contact_information"></el-input>
        </el-form-item>
        <el-form-item label="电子邮箱" prop="mail_box">
          <el-input v-model="ruleForm.mail_box"></el-input>
        </el-form-item>
        <el-form-item label="Q Q号">
          <el-input v-model="ruleForm.qq_number"></el-input>
        </el-form-item>
        <!-- <el-form-item label="回调网址" prop="returnurl">
          <el-input v-model="ruleForm.returnurl" placeholder="格式：http://www.baidu.com"></el-input>
        </el-form-item> -->
        <el-form-item label="到期时间" prop="expiration_date">
          <el-date-picker style="width:100%" v-model="ruleForm.expiration_date" type="datetime" placeholder="选择日期时间">
          </el-date-picker>
        </el-form-item>
        <el-form-item label="营业开始时间" prop="enabletime">
          <el-time-select style="width:100%" v-model="ruleForm.enabletime" :picker-options="{
              start: '06:00',
              step: '00:10',
              end: '23:59'
            }" placeholder="选择时间">
          </el-time-select>
          <!-- <el-date-picker
            style="width:100%"
            v-model="ruleForm.enabletime"
            type="datetime"
            placeholder="选择日期时间">
          </el-date-picker> -->
        </el-form-item>
        <el-form-item label="营业结束时间" prop="disabletime">
          <el-time-select style="width:100%" v-model="ruleForm.disabletime" :picker-options="{
              start: '06:00',
              step: '00:10',
              end: '23:59'
            }" placeholder="选择时间">
          </el-time-select>
          <!-- <el-date-picker
            style="width:100%"
            v-model="ruleForm.disabletime"
            type="datetime"
            placeholder="选择日期时间">
          </el-date-picker> -->
        </el-form-item>
        <!-- <el-form-item label="每日支付限额" prop="payment_limit">
          <el-input v-model.number="ruleForm.payment_limit"></el-input>
        </el-form-item>
        <el-form-item label="会员卡单日充值限额" prop="recharge_limit">
          <el-input v-model.number="ruleForm.recharge_limit"></el-input>
        </el-form-item> -->
        <el-form-item label="公司Logo" prop="company_logo">
          <el-upload class="avatar-uploader" :action="baseurl+'Upload'" :show-file-list="false" :on-success="handleAvatarSuccess" :before-upload="beforeAvatarUpload" :headers="{'Authorization':agToken}">
            <img v-if="ruleForm.company_logo" :src="ruleForm.company_logo" class="avatar">
            <i v-else class="el-icon-plus avatar-uploader-icon"></i>
          </el-upload>
        </el-form-item>
        <el-form-item label="域名" prop="domain">
          <el-input v-model="ruleForm.domain" placeholder="例如：www.baidu.com"></el-input>
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
  agent,
  merchant,
  editmerchant,
  GetBusids,
  setCashid
} from '@/api/user.js'
import { getKey } from '@/api/java_user.js'
import { validateURL, validatenumber } from '@/utils/validate.js'
import md5 from 'js-md5'
import { getToken } from '@/utils/auth'

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
    var checkAgId = (rule, value, callback) => {
      //代理商
      if (!value) {
        return callback(new Error('请选择代理商'))
      } else {
        callback()
      }
    }
    var checkpayment_limit = (rule, value, callback) => {
      if (!value) {
        return callback(new Error('请输入每日支付限额'))
      }
      setTimeout(() => {
        if (!Number.isInteger(value)) {
          callback(new Error('请输入数字值'))
        } else {
          if (value > 5000) {
            callback(new Error('每日支付限额不能大于5000'))
          } else {
            callback()
          }
        }
      }, 300)
    }
    var checkrecharge_limit = (rule, value, callback) => {
      if (!value) {
        return callback(new Error('请输入会员卡单日充值限额'))
      }
      setTimeout(() => {
        if (!Number.isInteger(value)) {
          callback(new Error('请输入数字值'))
        } else {
          if (value > 2000) {
            callback(new Error('会员卡单日充值限额不能大于2000'))
          } else {
            callback()
          }
        }
      }, 300)
    }
    const validatereturnurl = (rule, value, callback) => {
      if (value && !validateURL(value)) {
        callback(new Error('请输入正确的回调网址'))
      } else {
        callback()
      }
    }
    var number_change = (rule, value, callback) => {
      if (!validatenumber(value)) {
        callback(new Error('请输入数值类型'))
      } else {
        callback()
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
      multipleSelection: [],
      role: '',
      agentlist: [],
      ruleForm: '',
      dialogFormVisible: false,
      dialogStatus: '',
      textMap: {
        update: '编辑',
        create: '创建'
      },
      dialogPvVisible: false,
      rules: {
        name: [{ required: true, message: '请输入用户名称', trigger: 'blur' }],
        password: [{ validator: validatePass, trigger: 'blur' }],
        agid: [{ validator: checkAgId, trigger: 'change' }],
        contacts: [
          { required: true, message: '请输入联系人', trigger: 'blur' }
        ],
        appid: [{ required: true, message: '请输入商户号', trigger: 'blur' }],
        contact_information: [
          { required: true, message: '请输入联系方式', trigger: 'blur' }
        ],
        // expiration_date: [{ required: true, message: "请选择到期时间", trigger: "change" }],
        payment_limit: [{ validator: checkpayment_limit, trigger: 'blur' }],
        recharge_limit: [{ validator: checkrecharge_limit, trigger: 'blur' }],
        enabletime: [
          { required: true, message: '请选择营业开始时间', trigger: 'change' }
        ],
        disabletime: [
          { required: true, message: '请选择营业结束时间', trigger: 'change' }
        ],
        returnurl: [
          { required: true, trigger: 'blur', validator: validatereturnurl }
        ],
        comm_ratio: [
          { required: true, trigger: 'blur', validator: number_change }
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
    this.getagent()
  },
  methods: {
    resetTemp() {
      this.ruleForm = {
        id: '', //商户id
        name: '', //商户名称
        password: '', //密码
        agid: '', //代理商id
        user_status: 1, //状态
        user_rights: '', //用户权限
        merchant_name: '', //商户名称
        province: '', //省
        city: '', //市
        area: '', //区
        merchant_address: '', //商户地址
        contacts: '', //联系人
        contact_information: '', //联系方式
        mail_box: '', //邮　箱
        qq_number: '', //Q Q号
        disabletime: '', //营业结束时间
        enabletime: '', //营业开始时间
        expiration_date: '', //到期时间
        payment_limit: '', //每日支付限额
        recharge_limit: '', //会员卡单日充值限额
        returnurl: '', //回调网址
        comm_amount: '',
        comm_ratio: '',
        company_logo:'',
        domain:''
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
      this.ruleForm.password = ''
      this.dialogStatus = 'update'
      this.dialogFormVisible = true
    },
    handleToRoute(row) {
      this.$router.push({
        name: '商户名称数据分析',
        params: { merchantid: row.id }
      })
    },
    handleSelectionChange(val) {
      this.multipleSelection = val
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
    handleAudit(row) {
      this.$confirm('此操作审核大商户下所有商户名称, 是否继续?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).then(() => {
        this.ruleForm = Object.assign({}, row)
        var merchantids = []
        for (var item of this.multipleSelection) {
          merchantids.push(item.id)
        }
        GetBusids('get', { ids: merchantids.toString() }).then(response => {
          let success_count = 0
          let fail_count = 0
          this.$notify({
            title: '成功',
            message: `批量审核提交成功`,
            type: 'success',
            duration: 5000
          })
          for (var bus of response.data) {
            getKey('get', {
              data: {
                id: bus.id,
                username: bus.cashcode,
                password: bus.cashpwd
              }
            }).then(res => {
              if (res.data.data.statue == '1') {
                let data = res.data.data.data
                var item_data = {}
                item_data.id = data.id
                item_data.cashier_num = data.cashier_num
                item_data.merchant_num = data.merchant_num
                item_data.cash_token = data.token
                setCashid('put', item_data).then(res => {
                  success_count = success_count + 1
                })
              } else {
                fail_count = fail_count + 1
              }
            })
          }
        })
      })
    },
    handleDelete(row) {
      this.$confirm('此操作删除商家, 是否继续?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).then(() => {
        this.ruleForm = Object.assign({}, row)
        merchant('delete', { id: this.ruleForm.id }).then(response => {
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
      merchant('get', this.listQuery).then(response => {
        this.list = response.data
        this.total = response.total

        this.listLoading = false
      })
      // console.log(bData);
      // this.list = bData.router;
      // this.total = bData.router.length;
      // this.listLoading = false;
    },
    getagent() {
      this.listLoading = true
      agent('get', { page: 1, pagesize: 9999, code: '' }).then(response => {
        this.agentlist = response.data
      })
      // console.log(bData);
      // this.list = bData.router;
      // this.total = bData.router.length;
      // this.listLoading = false;
    },
    tagStatus(row) {
      this.ruleForm = Object.assign({}, row)
      this.ruleForm.password = ''
      this.ruleForm.user_status = this.ruleForm.user_status == '1' ? '0' : '1'
      this.submitupdate()
    },
    update(formName) {
      this.$refs[formName].validate(valid => {
        if (valid) {
          this.submitupdate()
        } else {
          console.log('error submit!!')
          return false
        }
      })
    },
    submitupdate() {
      editmerchant('put', this.ruleForm).then(response => {
        this.dialogFormVisible = false
        this.fetchData()
        this.$notify({
          title: '成功',
          message: '修改成功',
          type: 'success',
          duration: 2000
        })
      })
    },
    create(formName) {
      this.$refs[formName].validate(valid => {
        if (valid) {
          this.ruleForm.meta = this.getmeta
          editmerchant('post', this.ruleForm).then(response => {
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
