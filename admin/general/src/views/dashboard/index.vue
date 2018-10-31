<template>
  <div class="dashboard-container" :style="'height:'+fullHeight+'px'">
    <div class='dashboard-text'>欢迎登录易通付支付系统</div>
  </div>
</template>

<script>
import { mapGetters } from 'vuex'
export default {
  name: 'dashboard',
  data() {
    return {
      fullHeight: document.documentElement.clientHeight
    }
  },
  computed: {
    ...mapGetters(['name', 'roles'])
  }
}
</script>

<style rel="stylesheet/scss" lang="scss" scoped>
.dashboard {
  &-container {
    margin: 30px;
  }
  &-text {
    font-size: 30px;
    line-height: 46px;
    padding-top: 20px;
  }
}
</style>
