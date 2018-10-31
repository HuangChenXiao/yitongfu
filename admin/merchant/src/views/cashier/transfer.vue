<template>
  <div>
    <form id="forms" style="display:none">
      <!-- <input type="text" name="data" id="data" /> -->
    </form>
    {{err_msg}}
    <div style="text-align:center" v-if="payUrl">
      <span style="display: inline-block;color:red;margin:20px 0">{{paytype}}</span>
      <qriously id="mTarget" :value="payUrl" :backgroundAlpha="1" :size="300" />
    </div>
  </div>
</template>

<script>
// import { qrOrder } from '@/api/java_order.js'
export default {
  data() {
    return {
      bItem: JSON.parse(this.$route.params.str_data),
      err_msg: null,
      payUrl: null,
      paytype:""
    }
  },
  mounted() {
    qrOrder('post', { data: this.bItem }).then(res => {
      if (res.data.statue == 1) {
        if (this.bItem.businesspasstype == 1) {
          // $('#forms').attr('action', res.data.payUrl)
          // $('#forms').attr('method', 'get')
          // $('#forms').submit()
          location.replace(res.data.payUrl);
          // location.href=res.data.payUrl
        } else {
          this.payUrl = res.data.payUrl
          this.paytype = this.paytype_filters(this.bItem.paytype)
        }
      } else {
        this.err_msg = res.data
      }
    })
  },
  methods: {
    paytype_filters(val) {
      var valMap = {
        1: '请使用微信扫一扫',
        2: '请使用支付宝扫一扫'
      }
      return valMap[val]
    },
    IsWeixinOrAlipay() {
      var that = this
      var ua = window.navigator.userAgent.toLowerCase()
      //判断是不是微信
      if (ua.match(/MicroMessenger/i) == 'micromessenger') {
        that.bItem.paytype = 1
      }
      //判断是不是支付宝
      if (ua.match(/AlipayClient/i) == 'alipayclient') {
        that.bItem.paytype = 2
      }
      //哪个都不是
      return ''
    }
  }
}
</script>

<style scoped>
</style>