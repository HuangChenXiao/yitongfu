<template>
  <div class="gathering-content">
    <div class="gathering-item title">收银台
      <a href="javascript:;" style="color:green" @click="getLink()">
        <i class="el-icon-success"></i>手机端：</a>
      <a :href="str_external" style="color:green" target="_blank"><span v-if="str_external">点击前往</span></a>
      <!-- <div class="alipay-msg">
        温馨提示：★付款时请输入18位付款码
      </div> -->
    </div>

    <audio src="/static/msg.wav" controls="controls" id="audio_1" style="display:none">
      Your browser does not support the audio element.
    </audio>

    <el-form :model="ruleForm" :rules="rules" ref="ruleForm" class="demo-ruleForm" label-position="left" label-width="100px" style="width:500px;margin:0 auto">
      <el-form-item label="外链二维码" v-if="str_external" style="border-bottom:1px solid #ddd">
        <div style="text-align:center;">

          <el-select v-model="m_qrcode_size" placeholder="请选择二维码大小">
            <el-option :key="100" label="100像素" :value="100"></el-option>
            <el-option :key="200" label="200像素" :value="200"></el-option>
            <el-option :key="300" label="300像素" :value="300"></el-option>
            <el-option :key="500" label="500像素" :value="500"></el-option>
            <el-option :key="1000" label="1000像素" :value="1000"></el-option>
          </el-select>
          <el-button @click="mSaveImg()" style=";margin:20px 0;">下载二维码</el-button>
          <qriously id="mTarget" :value="str_external" :backgroundAlpha="1" :size="300" />
        </div>
      </el-form-item>
      <el-form-item required v-if="initQCode">
        <div style="text-align:center;">
          <el-select v-model="qr_size" placeholder="请选择二维码大小">
            <el-option :key="100" label="100像素" :value="100"></el-option>
            <el-option :key="200" label="200像素" :value="200"></el-option>
            <el-option :key="300" label="300像素" :value="300"></el-option>
            <el-option :key="500" label="500像素" :value="500"></el-option>
            <el-option :key="1000" label="1000像素" :value="1000"></el-option>
          </el-select>
          <el-button @click="saveImg()" style=";margin:20px 0;">下载二维码</el-button>
          <qriously id="target" :value="initQCode" backgroundAlpha="1" :size="qr_size" />
        </div>
      </el-form-item>
      <el-form-item prop="amount" required label="充值金额：">
        <el-input v-model="ruleForm.amount" auto-complete="off" placeholder="请输入收款金额" type="number"></el-input>
      </el-form-item>
      <el-form-item prop="remark" required label="输入会员：">
        <el-input v-model="ruleForm.remark" auto-complete="off" placeholder="备注" type="text"></el-input>
      </el-form-item>
      <!-- <el-form-item label="类型">
        <el-radio-group v-model="type">
          <el-radio label="2">扫码支付</el-radio>
          <el-radio label="1">18位付款码</el-radio>
        </el-radio-group>
      </el-form-item>
       -->
      <el-form-item label="支付类型">
        <el-radio-group v-model="ruleForm.paytype">
          <el-radio :label="2">支付宝</el-radio>
          <el-radio :label="1">微信</el-radio>
        </el-radio-group>
      </el-form-item>
      <el-form-item>
        <el-button type="primary" @click="submitForm('ruleForm')" style="width:100%">提交</el-button>
      </el-form-item>
    </el-form>
    <div class="gathering-item paycard-title">刷卡收款记录</div>
    <el-table v-loading.body="listLoading" :data="tableData3" max-height="1000" border style="width: 100%">
      <el-table-column align="center" label='序号' width="95">
        <template scope="scope">
          {{scope.$index +1}}
        </template>
      </el-table-column>
      <el-table-column label="订单号">
        <template scope="scope">
          <span>{{scope.row.orderno}}</span>
        </template>
      </el-table-column>
      <el-table-column label="订单金额" align="center">
        <template scope="scope">
          <span>{{scope.row.amount}}</span>
        </template>
      </el-table-column>
      <el-table-column label="订单状态" align="center">
        <template scope="scope">
          <el-tag :type="scope.row.status==1?'success':''">{{scope.row.status| statusFilter}}</el-tag>
        </template>
      </el-table-column>
      <el-table-column label="代理商" align="center">
        <template scope="scope">
          {{scope.row.agname}}
        </template>
      </el-table-column>
      <!-- <el-table-column label="虚拟商户" align="center">
        <template scope="scope">
          {{scope.row.merchantname}}
        </template>
      </el-table-column> -->
      <el-table-column label="商户名称" align="center">
        <template scope="scope">
          {{scope.row.shortname}}
        </template>
      </el-table-column>
      <el-table-column label="付款类型" align="center">
        <template scope="scope">
          <el-tag type="success">{{scope.row.paytype| orderTypeFilter}}</el-tag>
        </template>
      </el-table-column>
      <el-table-column label="备注" align="center">
        <template scope="scope">
          {{scope.row.remark}}
        </template>
      </el-table-column>
      <el-table-column label="订单时间" align="center">
        <template scope="scope">
          {{scope.row.addtime|dateFilter}}
        </template>
      </el-table-column>
    </el-table>

    <!-- <div v-show="!listLoading" class="pagination-container">
      <el-pagination @size-change="handleSizeChange" @current-change="handleCurrentChange" :current-page.sync="listQuery.page"
        :page-sizes="[10,20,30, 50]" :page-size="listQuery.limit" layout="total, sizes, prev, pager, next, jumper" :total="total">
      </el-pagination>
    </div> -->

    <div class="mask-container" v-show="dialogFormVisible" @click="dialogFormVisible=false">
      <div class="mask-content" @click.stop>
        <!-- <div class="mask-title">让用户打开[微信/支付宝][钱包][付款码]</div> -->
        <div class="mask-code-title">打开微信、支付宝，获取支付码并输入</div>
        <div class="mask-input">
          <el-input auto-complete="off" v-model="ruleForm.dynamic_id" placeholder="请输入付款码" style="font-size:18px;height:40px" @keyup.enter.native="payment_fun"></el-input>
        </div>
        <div class="mask-input" style="margin-top:1rem">
          <el-button type="primary" @click="payment_fun" style="width:100%" :loading="pay_btnloading">确认</el-button>
        </div>
      </div>
    </div>

    <div class="mask-container" v-show="dialogQrCodeVisible" @click="dialogQrCodeVisible=false">
      <div class="mask-content" @click.stop>
        <!-- <div class="mask-title">让用户打开[微信/支付宝][钱包][付款码]</div> -->
        <div class="mask-code-title">打开微信、支付宝，扫一扫支付</div>
        <div class="qrcode-img"><img :src="ruleForm.qr_url" alt="暂无二维码"></div>
        <div class="mask-input" style="margin-top:1rem">
          <el-button type="primary" @click="qrcodepayment_fun" :loading="qrcode_btn" style="width:100%">重新生成支付二维码</el-button>
        </div>
      </div>
    </div>
    <flow-mask v-model="flow_show"></flow-mask>
  </div>
</template>


<script>
import { getToken, setToken, removeToken } from '@/utils/auth'
import { mapGetters } from 'vuex'
import { getList } from '@/api/order'
import md5 from 'js-md5'
import flowMask from '@/views/flow'
import { createorder } from '@/api/java_order.js'

export default {
  components: {
    flowMask
  },
  data() {
    var validateamount = (rule, value, callback) => {
      if (value === '') {
        callback(new Error('请输入收款金额'))
      } else {
        var reg = /^\d+(\.([1-9]|\d[1-9]))?$/
        if (!reg.test(value)) {
          callback(new Error('收款金额最多保留两位小数'))
        } else {
          if (value > this.payable_amount) {
            this.ruleForm.amount = ''
            if (this.ruleForm.paytype == 3) {
              callback(new Error('金额上限为' + this.payable_amount + ''))
            } else {
              callback(new Error('金额上限为' + this.payable_amount + ''))
            }
          } else {
            callback()
          }
        }
      }
    }
    return {
      m_qrcode_size: 300,
      flow_show: false,
      type: '2',
      paymenttype: '1',
      qr_size: 300,
      initQCode: '',
      listLoading: true,
      listQuery: {
        page: 1,
        pagesize: 10,
        status: '3',
        begindate: this.cfg.getCurrentMonthFirst(),
        enddate: this.cfg.getCurrentMonthLast(),
        agid: 0,
        merchantid: 0,
        keyword: ''
      },
      total: 10,
      dialogFormVisible: false,
      dialogQrCodeVisible: false,
      ruleForm: {
        amount: '',
        remark: '',
        dynamic_id: '',
        qr_url: '',
        merchantid: 0,
        adduser: '',
        paytype: 2
      },
      rules: {
        amount: [{ validator: validateamount, trigger: 'blur' }],
        remark: [{ required: true, message: '请输入备注', trigger: 'blur' }]
      },
      tableData3: [],
      pay_btnloading: false,
      qrcode_btn: false,
      str_external: ''
    }
  },
  computed: {
    ...mapGetters(['user_id', 'user_name', 'appid', 'appsecret']),
    payable_amount() {
      if (this.ruleForm.paytype == 3) {
        return 20000
      } else {
        return 2000
      }
    }
  },
  filters: {
    statusFilter(status) {
      const statusMap = {
        0: '未支付',
        1: '支付完成',
        2: '支付失败',
        3: '全部'
      }
      return statusMap[status]
    },
    dateFilter(value) {
      if (value) {
        return value.replace('T', ' ')
      }
    },
    orderTypeFilter(paytype) {
      const paytypeMap = {
        1: '微信',
        2: '支付宝'
      }
      return paytypeMap[paytype]
    }
  },
  created() {
    this.ruleForm.merchantid = this.user_id
    this.ruleForm.adduser = this.user_name
    this.getList()
    // this.initQCode =
    //   'http://pay.niuchao.net/paySuccess?appid=' + this.appid + ''
  },
  methods: {
    getLink() {
      console.log(window.location.host)
      this.str_external =
        'http://' +
        window.location.host +
        '/#/external/' +
        this.appid 
    },
    saveImg() {
      var canvasData = $('#target').children('canvas')
      var a = document.createElement('a')
      a.href = canvasData[0].toDataURL()
      a.download = 'drcQrcode'
      a.click()
    },
    mSaveImg() {
      var canvasData = $('#mTarget').children('canvas')
      var a = document.createElement('a')
      a.href = canvasData[0].toDataURL()
      a.download = 'drcQrcode'
      a.click()
    },
    getList() {
      getList('get', this.listQuery).then(response => {
        this.tableData3 = response.data
        this.total = response.data != null ? response.data[0].count : 0

        this.listLoading = false
      })
    },
    submitForm(formName) {
      this.$refs[formName].validate(valid => {
        if (valid) {
          if (this.type == 1) {
            this.$confirm('付款前请先提取18位的付款码！', '付款提示', {
              confirmButtonText: '确定',
              cancelButtonText: '取消',
              type: 'warning'
            })
              .then(() => {
                this.dialogFormVisible = true
              })
              .catch(() => {
                this.$message({
                  type: 'info',
                  message: '已取消付款'
                })
              })
          } else {
            var str_data = {}
            str_data.amount = this.ruleForm.amount
            str_data.appid = this.appid
            str_data.remark = this.ruleForm.remark
            str_data.paytype = this.ruleForm.paytype
            this.initQCode =
              this.java_baseurl +
              'yt_pay/pay/createorder.do?' +
              this.cfg.parseParams(str_data)
            // createorder('post', str_data).then(res => {
            //   if (res.data.statue == 1) {
            //     this.initQCode = res.data.payUrl
            //   } else {
            //     this.$message({
            //       message: res.data.message,
            //       type: 'warning'
            //     })
            //     return
            //   }
            // })
          }
        } else {
          console.log('error submit!!')
          return false
        }
      })
    },
    resetForm(formName) {
      this.$refs[formName].resetFields()
    },
    handleSizeChange(val) {
      this.listQuery.limit = val
      this.getList()
    },
    handleCurrentChange(val) {
      this.listQuery.page = val
      this.getList()
    },
    payment_fun() {
      if (!this.ruleForm.dynamic_id) {
        this.$message({
          message: '请输入收款码',
          type: 'warning'
        })
        return
      }
      this.pay_btnloading = true
      authcodePayXY('get', { data: this.ruleForm })
        .then(res => {
          var status_code = res.data.data.statue
          var message = res.data.data.message
          if (status_code == 1) {
            this.$message({
              message: '订单发起成功，等待收款！',
              type: 'success',
              duration: 5000
            })
            // if (res.data.data.data.RESP_CODE == '0000') {
            //   var myAudio = document.getElementById('audio_1')
            //   myAudio.play()
            // }
            this.ruleForm.dynamic_id = null
            this.pay_btnloading = false
            this.dialogFormVisible = false
            setTimeout(() => {
              this.getList()
            }, 2000)
            return
          } else {
            this.pay_btnloading = false
            this.$message({
              message: message,
              type: 'error'
            })
          }
        })
        .catch(res => {
          this.pay_btnloading = false
          this.$message({
            message: '收款失败',
            type: 'error'
          })
        })
    },
    qrcodepayment_fun() {
      this.qrcode_btn = true
      alipayOrWxpayPrecreateXY('get', { data: this.ruleForm })
        .then(res => {
          var status_code = res.data.data.statue
          var message = res.data.data.message
          if (status_code == 1) {
            this.ruleForm.qr_url = res.data.data.QRcode
            this.qrcode_btn = false
            return
          } else {
            this.qrcode_btn = false
            this.$message({
              message: message,
              type: 'error'
            })
          }
        })
        .catch(res => {
          this.$message({
            message: '生成二维码失败',
            type: 'error'
          })
        })
    }
  },
  watch: {
    paymenttype(val, oldVal) {
      if (val == 1) {
        this.ruleForm.bus_code = '3101'
      } else if (val == 2) {
        this.ruleForm.bus_code = '3201'
      } else if (val == 3) {
        this.ruleForm.bus_code = '5019'
      } else if (val == 4) {
        this.ruleForm.bus_code = '5011'
      }
    },
    dialogQrCodeVisible(val, oldVal) {
      if (!val) {
        this.getList()
      }
    }
  }
}
</script>
<style scoped>
.gathering-content {
  width: 80%;
  border: 1px solid #e5e5e5;
  margin: 0 auto;
  margin-top: 20px;
  background: #fff;
}
.gathering-item {
  position: relative;
  font-size: 18px;
  padding: 20px 50px;
}
.title {
  border-bottom: 1px solid #e5e5e5;
  margin-bottom: 20px;
}
.paycard-title {
  border-top: 1px solid #e5e5e5;
  border-bottom: 1px solid #e5e5e5;
  margin-bottom: 20px;
}
.mask-container {
  position: fixed;
  top: 0;
  left: 0;
  z-index: 999;
  width: 100%;
  height: 100%;
  background: rgba(0, 0, 0, 0.6);
}
.mask-container .mask-content {
  background: #fff;
  margin: 0 auto;
  margin-top: 15%;
  width: 600px;
  border-radius: 5px;
  padding: 30px 50px;
}
.mask-container .mask-content .mask-title {
  color: #ee6060;
  height: 40px;
  line-height: 40px;
  text-align: center;
}
.mask-container .mask-content .mask-code-title {
  color: #666;
  height: 40px;
  line-height: 40px;
  text-align: center;
}
.mask-container .mask-content .mask-input {
  color: #666;
  height: 40px;
  line-height: 40px;
  text-align: center;
  width: 300px;
  margin: 0 auto;
}
.help {
  position: absolute;
  right: 20px;
  color: rgb(64, 158, 255);
}
.qrcode-img {
  width: 100%;
  text-align: center;
}
.alipay-msg {
  text-align: center;
  color: red;
}
</style>
