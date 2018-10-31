<template>
  <el-menu class="navbar" mode="horizontal">
    <!-- <hamburger class="hamburger-container" :toggleClick="toggleSideBar" :isActive="sidebar.opened"></hamburger> -->
    <levelbar></levelbar>
    <!-- <el-dropdown class="avatar-container" trigger="click">
      <div class="avatar-wrapper">
        <img class="user-avatar" src="https://wpimg.wallstcn.com/f778738c-e4f8-4870-b634-56703b4acafe.gif?imageView2/1/w/80/h/80">
        <i class="el-icon-caret-bottom"></i>
      </div>
      <el-dropdown-menu class="user-dropdown" slot="dropdown">
        <router-link class='inlineBlock' to="/">
          <el-dropdown-item>
            首页
          </el-dropdown-item>
        </router-link>
        <el-dropdown-item divided><span @click="logout" style="display:block;">退出登录</span></el-dropdown-item>
      </el-dropdown-menu>
    </el-dropdown> -->
    <div class="avatar-container">
      <el-button @click="handleUpdate">修改密码</el-button>
      <el-button><a href="static/doc/pay.doc" class="down-doc">下载开发文档</a></el-button>
      <el-button @click="logout" >安全退出</el-button>
    </div>
    <el-dialog title="修改密码" :visible.sync="dialogFormVisible" width="50%">
      <el-form class="small-space" :model="ruleForm" :rules="rules" ref="ruleForm" label-position="left" label-width="150px" style='width: 620px; margin-left:50px;'>
        <el-form-item label="密码" prop="password">
          <el-input v-model="ruleForm.password" type="password"></el-input>
        </el-form-item>
      </el-form>
      <span slot="footer" class="dialog-footer">
        <el-button @click="dialogFormVisible = false">取 消</el-button>
        <el-button type="primary" @click="update_password('ruleForm')">确 定</el-button>
      </span>
    </el-dialog>
  </el-menu>
</template>

<script>
import { mapGetters } from 'vuex'
import Levelbar from './Levelbar'
import Hamburger from '@/components/Hamburger'
import { UpdatePassword } from '@/api/merchant.js'
import { removeToken } from '@/utils/auth.js'

export default {
  components: {
    Levelbar,
    Hamburger
  },
  data() {
    const validatePass = (rule, value, callback) => {
      if (value.length < 5) {
        callback(new Error('密码不能小于5位'))
      } else {
        callback()
      }
    }
    return {
      dialogFormVisible: false,
      rules: {
        password: [{ required: true, trigger: 'blur', validator: validatePass }]
      },
      ruleForm: {
        password: ''
      }
    }
  },
  computed: {
    ...mapGetters(['sidebar', 'avatar', 'user_id'])
  },
  methods: {
    resetForm() {
      this.ruleForm = {
        password: ''
      }
    },
    handleUpdate() {
      this.dialogFormVisible = true
      this.resetForm()
      console.log(this.ruleForm)
    },
    update_password(formName) {
      var _this = this
      this.$refs[formName].validate(valid => {
        if (valid) {
          var data = {
            id: this.user_id,
            password: this.ruleForm.password,
            type: 2
          }
          UpdatePassword('get', data).then(res => {
            this.$notify({
              title: '成功',
              message: '修改成功',
              type: 'success',
              duration: 800,
              onClose: function() {
                removeToken()
                location.reload()
              }
            })
          })
        } else {
          console.log('error submit!!')
          return false
        }
      })
    },
    toggleSideBar() {
      this.$store.dispatch('ToggleSideBar')
    },
    logout() {
      this.$store.dispatch('LogOut').then(() => {
        location.reload() // 为了重新实例化vue-router对象 避免bug
      })
    }
  }
}
</script>

<style rel="stylesheet/scss" lang="scss" scoped>
.navbar {
  height: 50px;
  line-height: 50px;
  border-radius: 0px !important;
  border-top: 1px solid #efefef;
  border-left: 1px solid #efefef;
  border-bottom: 1px solid #efefef;
  background: #fff;
  .hamburger-container {
    line-height: 58px;
    height: 50px;
    float: left;
    padding: 0 10px;
  }
  .errLog-container {
    display: inline-block;
    position: absolute;
    right: 150px;
  }
  .screenfull {
    position: absolute;
    right: 90px;
    top: 16px;
    color: red;
  }
  .avatar-container {
    height: 50px;
    display: inline-block;
    position: absolute;
    right: 35px;
    .avatar-wrapper {
      cursor: pointer;
      margin-top: 5px;
      position: relative;
      .user-avatar {
        width: 40px;
        height: 40px;
        border-radius: 10px;
      }
      .el-icon-caret-bottom {
        position: absolute;
        right: -20px;
        top: 25px;
        font-size: 12px;
      }
    }
  }
}
</style>

