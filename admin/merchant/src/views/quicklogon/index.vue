<template>
    <div>
    </div>
</template>
<script>
export default {
  data() {
    return {
      loginForm: {
        username: this.$route.params.username,
        password: this.$route.params.password,
        sign:this.$route.params.sign,
      },
    }
  },
  created(){
    this.$store.dispatch('MerchantQuickLogon', this.loginForm).then(() => {
        // location.reload();
        this.$router.push({ path: '/' })
    }).catch(() => {
       alert("登录失败，用户名或密码错误");
       this.$router.push({ path: '/login' })
    })
  }
}
</script>