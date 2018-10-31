<template>
    <div class="login-body" :style="'height:'+fullHeight+'px'">
        <div class="login">
            <div class="message">易通付-管理登录</div>
            <div id="darkbannerwrap"></div>

            <form method="post">
                <input name="action" value="login" type="hidden">
                <!-- <input name="username" placeholder="用户名" required="" type="text"> -->
                <el-input name="username" class="login-ipt" type="text" v-model="loginForm.username" autoComplete="on" placeholder="请输入账号" />
                <hr class="hr15">
                <el-input name="password" class="login-ipt" type="password" @keyup.enter.native="handleLogin" v-model="loginForm.password" autoComplete="on" placeholder="请输入密码"></el-input>
                <hr class="hr15">
                <!-- <input value="登录" style="width:100%;" type="submit"> -->
                <el-button type="primary" class="login-btn" style="width:100%;" :loading="loading" @click.native.prevent="handleLogin">
                    登 录
                </el-button>
                <hr class="hr20">
            </form>

        </div>
    </div>
</template>

<script>
// export default {
//   data() {
//     return {
//       fullHeight: document.documentElement.clientHeight
//     }
//   }
// }
export default {
  name: 'login',
  data() {
    const validateUsername = (rule, value, callback) => {
      if (!value) {
        callback(new Error('请输入正确的用户名'))
      } else {
        callback()
      }
    }
    const validatePass = (rule, value, callback) => {
      if (value.length < 5) {
        callback(new Error('密码不能小于5位'))
      } else {
        callback()
      }
    }
    return {
      fullHeight: document.documentElement.clientHeight,
      loginForm: {
        username: '',
        password: ''
      },
      loginRules: {
        username: [
          { required: true, trigger: 'blur', validator: validateUsername }
        ],
        password: [{ required: true, trigger: 'blur', validator: validatePass }]
      },
      loading: false
    }
  },
  methods: {
    handleLogin() {
      if (!this.loginForm.username) {
        this.$message({
          message: '请输入用户名',
          type: 'warning'
        })
        return
      }
      if (!this.loginForm.password) {
        this.$message({
          message: '请输入密码',
          type: 'warning'
        })
        return
      }
      this.loading = true
      this.$store
        .dispatch('Login', this.loginForm)
        .then(() => {
          this.loading = false
          this.$router.push({ path: '/' })
        })
        .catch(() => {
          this.loading = false
        })
    }
  }
}
</script>
<style>
.login-ipt .el-input__inner {
  border: 1px solid #dcdee0;
  vertical-align: middle;
  border-radius: 3px;
  height: 50px;
  padding: 0px 16px;
  font-size: 14px;
  color: #555555;
  outline: none;
  width: 100%;
}
.login-btn {
  display: inline-block;
  vertical-align: middle;
  padding: 12px 24px;
  margin: 0px;
  font-size: 18px;
  line-height: 24px;
  text-align: center;
  white-space: nowrap;
  vertical-align: middle;
  cursor: pointer;
  color: #ffffff;
  background-color: #2ba245;
  border-radius: 3px;
  border: none;
  -webkit-appearance: none;
  outline: none;
  width: 100%;
}
</style>

<style scoped lang="scss">
.login-body {
  background: url(../../assets/img/web_login_bg.jpg) no-repeat center;
  background-size: cover;
  padding-top: 150px;
}
.login {
  min-height: 420px;
  max-width: 420px;
  padding: 40px;
  background-color: #ffffff;
  margin-left: auto;
  margin-right: auto;
  border-radius: 4px;
  /* overflow-x: hidden; */
  box-sizing: border-box;
}
.message {
  margin: 10px 0 0 -58px;
  padding: 18px 10px 18px 60px;
  background: #2ba245;
  position: relative;
  color: #fff;
  font-size: 16px;
}
#darkbannerwrap {
  background: url(../../assets/img/aiwrap.png);
  width: 18px;
  height: 10px;
  margin: 0 0 20px -58px;
  position: relative;
}

hr.hr15 {
  height: 15px;
  border: none;
  margin: 0px;
  padding: 0px;
  width: 100%;
}
hr.hr20 {
  height: 20px;
  border: none;
  margin: 0px;
  padding: 0px;
  width: 100%;
}
.copyright {
  font-size: 14px;
  color: rgba(255, 255, 255, 0.85);
  display: block;
  position: absolute;
  bottom: 15px;
  right: 15px;
}
a {
  color: #2ba245;
  text-decoration: none;
  cursor: pointer;
}
</style>