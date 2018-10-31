<template>
  <div class="gathering-content">
    <div class="gathering-item title">
      <div class="shouyin">收银台</div>
      <!-- <div class="msg">★提示：付款前请仔细阅读付款流程图</div> -->
    </div>
    <el-form :model="ruleForm" :rules="rules" ref="ruleForm" class="demo-ruleForm" label-position="left" label-width="100px" style="width:400px;margin:0 auto;position: relative;">
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
      </el-form-item> -->
      <el-form-item label="支付类型" v-show="type==2">
        <el-radio-group v-model="ruleForm.paytype" class="payType">
          <el-radio :label="2">支付宝</el-radio>
          <el-radio :label="1">微信</el-radio>
        </el-radio-group>
      </el-form-item>
      <el-form-item>
        <el-button type="primary" @click="SuccessForm('ruleForm')" class="submit_btn" style="width:100%">提交</el-button>
      </el-form-item>
      <el-form-item>
        <!-- <el-button type="text" class="submit_btn" style="width:100%" @click="flow_show=true">查看付款流程</el-button> -->
      </el-form-item>
    </el-form>
    <div class="mask-container" v-show="dialogFormVisible" @click="dialogFormVisible=false">
      <div class="mask-content" @click.stop>
        <!-- <div class="mask-title">让用户打开[微信/支付宝][钱包][付款码]</div> -->
        <div class="mask-code-title">打开微信、支付宝，获取支付码并输入</div>
        <div class="mask-input">
          <el-input auto-complete="off" v-model="ruleForm.dynamic_id" placeholder="请输入付款码" style="font-size:18px;height:40px" @keyup.enter.native="payment_fun"></el-input>
        </div>
        <div class="mask-input" style="margin-top:1rem">
          <el-button type="primary" @click="payment_fun" :loading="authcodebtn" style="width:100%">确认</el-button>
        </div>
      </div>
    </div>
    <div class="mask-container" v-show="dialogQrCodeVisible" @click="dialogQrCodeVisible=false">
      <div class="mask-content" @click.stop>
        <!-- <div class="mask-title">让用户打开[微信/支付宝][钱包][付款码]</div> -->
        <div class="mask-code-title">打开微信、支付宝，扫一扫支付</div>
        <div class="qrcode-img"><img :src="ruleForm.qr_url" alt="暂无二维码"></div>
        <div class="mask-input" style="margin-top:1rem">
          <el-button type="primary" @click="qrcodepayment_fun" :loading="qrcodebtn" style="width:100%">生成二维码</el-button>
        </div>
      </div>
    </div>
    <!-- <help></help> -->

    <el-dialog title="二维码付款" :show-close="false" :close-on-click-modal="false" :visible.sync="dialogMessageVisible" width="90%" :center="true">
      <div style="text-align:center" v-if="payUrl">
        <!-- <qriously id="mTarget" :value="payUrl" :backgroundAlpha="1" :size="300" /> -->
        <div id="mTarget" style="display:none"></div>
        <div id="imagQrDiv"></div>
      </div>
    </el-dialog>
    <flow-mask v-model="flow_show"></flow-mask>
    <form id="forms" style="display:none">
    </form>
  </div>
</template>


<script>
import help from '@/views/help/index.vue'
import flowMask from '@/views/flow'

export default {
  components: {
    help,
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
      flow_show: false,
      type: '2',
      paymenttype: '1',
      dialogFormVisible: false,
      dialogQrCodeVisible: false,
      payUrl: null,
      gethelp: '',
      ruleForm: {
        appid: this.$route.params.appid,
        amount: '',
        remark: '',
        dynamic_id: '',
        qr_url: '',
        paytype: 2
      },
      authcodebtn: false,
      qrcodebtn: false,
      dialogMessageVisible: false,
      submitMessage: false,
      rules: {
        amount: [{ validator: validateamount, trigger: 'blur' }],
        remark: [{ required: true, message: '请输入备注', trigger: 'blur' }]
      }
    }
  },
  computed: {
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
    }
  },
  mounted() {
    //截取问号前url
    var url = window.location.href
    if (url.indexOf('?') != -1) {
      url = url.split('?')[0]
      location.replace(url)
    }
  },
  methods: {
    submitForm(formName) {
      this.$refs[formName].validate(valid => {
        if (valid) {
          this.submitMessage = true
          this.dialogMessageVisible = false
        } else {
          console.log('error submit!!')
          return false
        }
      })
    },
    SuccessForm(formName) {
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
            str_data.appid = this.ruleForm.appid
            str_data.remark = this.ruleForm.remark
            str_data.paytype = this.ruleForm.paytype
            location.href =
              this.java_baseurl +
              'yt_pay/pay/createorder.do?' +
              this.cfg.parseParams(str_data)
          }
        } else {
          console.log('error submit!!')
          return false
        }
      })
    },
    convertCanvasToImage(canvas) {
      //新Image对象，可以理解为DOM
      var image = new Image()
      // canvas.toDataURL 返回的是一串Base64编码的URL，当然,浏览器自己肯定支持
      // 指定格式 PNG
      image.src = canvas.toDataURL('image/png')
      return image
    },
    payment_fun() {
      if (!this.ruleForm.dynamic_id) {
        this.$message({
          message: '请输入收款码',
          type: 'warning'
        })
        return
      }
      this.authcodebtn = true
      externalAuthcodePayXY('get', { data: this.ruleForm })
        .then(res => {
          var status_code = res.data.data.statue
          var message = res.data.data.message
          if (status_code == 1) {
            this.$message({
              message: '订单发起成功，等待收款！',
              type: 'success',
              duration: 5000
            })
            this.ruleForm = {
              amount: '',
              remark: '',
              dynamic_id: ''
            }
            this.authcodebtn = false
            this.dialogFormVisible = false
          } else {
            this.authcodebtn = false
            this.$message({
              message: message,
              type: 'error'
            })
          }
        })
        .catch(res => {
          this.authcodebtn = false
          this.$message({
            message: '收款失败',
            type: 'error'
          })
        })
    },
    qrcodepayment_fun() {
      this.qrcodebtn = true
      externalAlipayOrWxpayPrecreateXY('get', { data: this.ruleForm })
        .then(res => {
          var status_code = res.data.data.statue
          var message = res.data.data.message
          if (status_code == 1) {
            this.ruleForm.qr_url = res.data.data.QRcode
            this.qrcodebtn = false
            return
          } else {
            this.qrcodebtn = false
            this.$message({
              message: message,
              type: 'error'
            })
          }
        })
        .catch(res => {
          this.qrcodebtn = false
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
    }
  }
}
</script>

<style>
.el-radio-group {
  padding: 0 10px;
}
/* .el-radio-group .el-radio {
  width: 50%;
  float: left;
  margin: 5px;
} */
@media screen and (max-width: 650px) {
  .gathering-item .msg {
    position: relative !important;
    width: 100%;
    /* border: 1px solid #e5e5e5; */
    margin: 0 auto;
    left: 0 !important;
    /* height: 50px;
    line-height: 50px; */
    /* white-space: nowrap; */
    padding: 10px !important;
  }
  .mask-container .mask-content {
    width: 80% !important;
  }
  .mask-container .mask-content .mask-input {
    width: 100% !important;
  }
  .mask-container .mask-content {
    margin-top: 50% !important;
  }
  .el-message-box {
    width: 80%;
  }
  .el-input,
  .el-input__inner {
    width: 90%;
  }
  .submit_btn {
    width: 80% !important;
    margin-left: 10px;
  }
  .paytype {
    width: 80%;
  }
  ul li {
    width: 100% !important;
    margin-left: -6% !important;
  }
  form {
    width: 100% !important;
  }
  .el-form--label-left .el-form-item__label {
    margin-left: 10px;
  }
  .el-form-item__error {
    left: 10px;
  }
  .msg-phone {
    position: relative !important;
    right: -10px !important;
    white-space: normal !important;
  }
  .el-dialog--small {
    width: 90%;
    top: 30% !important;
  }
}
</style>


<style scoped>
.msg {
  position: absolute;
  width: 100%;
  /* border: 1px solid #e5e5e5; */
  margin: 0 auto;
  top: 0;
  left: 40%;
  color: red;
  font-size: 25px;
  /* white-space: nowrap; */
  padding: 10px 0;
}
.gathering-content {
  width: 90%;
  border: 1px solid #e5e5e5;
  margin: 0 auto;
  margin-top: 5%;
}
.gathering-item {
  font-size: 18px;
  /* padding: 20px 50px; */
}
.title {
  position: relative;
  /* border-bottom: 1px solid #e5e5e5; */
  margin-bottom: 20px;
  font-size: 20px;
  /* height: 50px;
  line-height: 50px; */
  /* padding-left: 20px; */
}
.title .shouyin {
  position: relative;
  border-bottom: 1px solid #e5e5e5;
  height: 50px;
  line-height: 50px;
  padding-left: 20px;
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
  padding: 10px 0;
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
.qrcode-img img {
  width: 80%;
}
.msg-phone {
  position: relative;
  right: -10px;
  top: 0px;
  color: red;
  font-size: 16px;
  white-space: nowrap;
}
</style>
