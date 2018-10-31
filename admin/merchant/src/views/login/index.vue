<template>
  <div>
    <div class="content-wrapper d-flex align-items-center auth" :style="'height:'+fullHeight+'px'">
      <div class="row w-100">
        <div class="col-lg-4 mx-auto">
          <div class="auth-form-light text-left p-5">
            <div class="brand-logo">
              <img :src="company_logo">
            </div>
            <h4>商户后台管理</h4>
            <form class="pt-3" style="width:400px">
              <div class="form-group">
                <input type="email" class="form-control form-control-lg" id="exampleInputEmail1" v-model="loginForm.username" placeholder="请输入用户名">
              </div>
              <div class="form-group">
                <input type="password" class="form-control form-control-lg" id="exampleInputPassword1" v-model="loginForm.password" placeholder="请输入密码">
              </div>
              <div class="mt-3">
                <el-button class="btn btn-block btn-gradient-primary btn-lg font-weight-medium auth-form-btn" type="primary" style="width:100%;" :loading="loading" @click.native.prevent="handleLogin">
                  登 录
                </el-button>
              </div>
            </form>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
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
      company_logo: this.$store.getters.company_logo
        ? this.$store.getters.company_logo
        : require('../../assets/img/logo.png'),
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

<style scoped lang="scss">
.content-wrapper {
  background: #f2edf3;
  padding: 2.75rem 2.25rem;
  width: 100%;
  -webkit-flex-grow: 1;
  flex-grow: 1;
}
.auth .brand-logo img {
  width: 150px;
}
.w-100 {
  width: 100% !important;
}
.row {
  display: flex;
  flex-wrap: wrap;
  margin-right: -20px;
  margin-left: -20px;
}
.ml-auto,
.mx-auto {
  margin-left: auto !important;
}
.mx-auto {
  margin-top: 100px;
}
.mr-auto,
.mx-auto {
  margin-right: auto !important;
}
.auth .auth-form-light {
  background: #ffffff;
}
.text-left {
  text-align: left !important;
}
.p-5 {
  padding: 3rem !important;
}
.auth .brand-logo {
  margin-bottom: 2rem;
}
h4,
.h4 {
  font-size: 1.13rem;
}
.pt-3,
.py-3 {
  padding-top: 1rem !important;
}
.auth form .form-group {
  margin-bottom: 1.5rem;
}
.auth form .form-group .form-control {
  background: transparent;
  border-radius: 0;
  font-size: 0.9375rem;
}
.form-control {
  border: 1px solid #ebedf2;
  font-family: 'ubuntu-regular', sans-serif;
  font-size: 0.8125rem;
}
.form-control {
  box-shadow: none;
}
.form-control {
  display: block;
  width: 100%;
  padding: 0.875rem 1.375rem;
  font-size: 1rem;
  line-height: 1;
  color: #495057;
  background-color: #ffffff;
  background-clip: padding-box;
  border: 1px solid #ced4da;
  border-radius: 2px;
  transition: border-color 0.15s ease-in-out, box-shadow 0.15s ease-in-out;
}
input,
.form-control:focus,
input:focus,
select:focus,
textarea:focus,
button:focus {
  outline: none;
  outline-width: 0;
  outline-color: transparent;
  box-shadow: none;
  outline-style: none;
}
.form-control-lg,
.input-group-lg > .form-control,
.input-group-lg > .input-group-prepend > .input-group-text,
.input-group-lg > .input-group-append > .input-group-text,
.input-group-lg > .input-group-prepend > .btn,
.input-group-lg > .input-group-append > .btn {
  padding: 0.94rem 1.94rem;
  font-size: 1.25rem;
  line-height: 1.5;
  border-radius: 0.3rem;
}
.mt-3,
.template-demo > .btn,
.template-demo > .btn-toolbar,
.my-3 {
  margin-top: 1rem !important;
}
.btn:not(:disabled):not(.disabled) {
  cursor: pointer;
}
.auth form .auth-form-btn {
  height: 50px;
  line-height: 1.5;
}
.btn-gradient-primary:not(.btn-gradient-light) {
  color: #ffffff;
}
.btn.btn-lg,
.btn-group-lg > .btn {
  font-size: 0.875rem;
}
.btn,
.btn-group.open .dropdown-toggle,
.btn:active,
.btn:focus,
.btn:hover,
.btn:visited,
a,
a:active,
a:checked,
a:focus,
a:hover,
a:visited,
body,
button,
button:active,
button:hover,
button:visited,
div,
input,
input:active,
input:focus,
input:hover,
input:visited,
select,
select:active,
select:focus,
select:visited,
textarea,
textarea:active,
textarea:focus,
textarea:hover,
textarea:visited {
  -webkit-box-shadow: none;
  -moz-box-shadow: none;
  box-shadow: none;
}
.btn-gradient-primary {
  background: #2ba245;
  border: 0;
  -webkit-transition: opacity 0.3s ease;
  -moz-transition: opacity 0.3s ease;
  -ms-transition: opacity 0.3s ease;
  -o-transition: opacity 0.3s ease;
  transition: opacity 0.3s ease;
}
.btn {
  font-size: 0.875rem;
  line-height: 1;
  font-family: 'ubuntu-bold', sans-serif;
}
.btn-block {
  display: block;
  width: 100%;
}

.btn-lg,
.btn-group-lg > .btn {
  padding: 1rem 3rem;
  font-size: 1.25rem;
  line-height: 1.5;
  border-radius: 0.1875rem;
}
.btn {
  display: inline-block;
  font-weight: 400;
  text-align: center;
  white-space: nowrap;
  vertical-align: middle;
  user-select: none;
  border: 1px solid transparent;
  padding: 0.875rem 2.5rem;
  font-size: 1rem;
  line-height: 1;
  border-radius: 0.1875rem;
  transition: color 0.15s ease-in-out, background-color 0.15s ease-in-out,
    border-color 0.15s ease-in-out, box-shadow 0.15s ease-in-out;
}
</style>