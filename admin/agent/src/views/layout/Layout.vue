<template>
  <div class="app-wrapper" :class="{hideSidebar:!sidebar.opened}">
    <div class="trademark clearfix">
      <div class="trademark-img">
        <img :src="company_logo" alt="">
      </div>
      <!-- <div class="company">
        欢迎使用支付后台管理系统
      </div> -->
      <div class="qq">
        <!-- QQ客服：3285448499 -->
        当前账号：
        <span>{{code}}</span>
        &nbsp;&nbsp;&nbsp;&nbsp;当前名称：
        <span>{{name}}</span>
        &nbsp;&nbsp;&nbsp;&nbsp;当前佣金：
        <span>{{commission>0?commission:0}}</span>
      </div>
    </div>
    <div class="sidebar-wrapper" id="sidebar-wrapper">
      <sidebar class="sidebar-container"></sidebar>
    </div>
    <div class="main-container">
      <navbar></navbar>
      <app-main></app-main>
    </div>
    <audio src="../../assets/audio/dx.mp3" controls="controls" preload id="music" hidden>
    </audio>
  </div>
</template>


<script>
import { Navbar, Sidebar, AppMain } from '@/views/layout'
import { mapGetters } from 'vuex'
import { despoitVoice } from '@/api/login.js'

export default {
  name: 'layout',
  components: {
    Navbar,
    Sidebar,
    AppMain
  },
  data() {
    return {
      name: '',
      code: '',
      commission: '',
      id: null,
      company_logo: this.$store.getters.company_logo
        ? this.$store.getters.company_logo
        : require( '../../assets/img/logo.png'),
    }
  },
  computed: {
    sidebar() {
      return this.$store.state.app.sidebar
    }
  },
  mounted() {
    this.$store
      .dispatch('GetInfo')
      .then(res => {
        this.name = res.data.contacts
        this.code = res.data.name
        this.commission = res.data.commission
        this.id = res.data.id
      })
      .catch(() => {})
    window.addEventListener('scroll', this.handleScroll)
    var that = this
    setInterval(() => {
      that.auto_despo()
    }, 60000)
  },
  methods: {
    auto_despo() {
      if (this.id) {
        despoitVoice('get', { agid: this.id }).then(res => {
          if (res.message == '1') {
            var audio = document.getElementById('music')
            audio.play() // 播放
            this.$router.push({
              path: '/withdrawals/withdrawals'
            })
          }
        })
      }
    },
    handleScroll() {
      this.scrolled = window.scrollY
      console.log(this.scrolled)
      if (this.scrolled >= 70) {
        $('.sidebar-wrapper').attr('style', 'top:0px !important')
      } else if (this.scrolled > 1 && this.scrolled < 70) {
        var ht = 70 - this.scrolled
        $('.sidebar-wrapper').attr('style', 'top:' + ht + 'px !important')
      } else {
        $('.sidebar-wrapper').attr('style', 'top:70px !important')
      }
    }
  }
}
</script>
<style>
.app-wrapper .sidebar-wrapper {
  top: 70px !important;
  z-index: 998 !important;
}
</style>

<style rel="stylesheet/scss" lang="scss" scoped>
@import 'src/styles/mixin.scss';
.app-wrapper {
  @include clearfix;
  position: relative;
  height: 100%;
  width: 100%;
  &.hideSidebar {
    .sidebar-wrapper {
      transform: translate(-140px, 0);
      .sidebar-container {
        transform: translate(132px, 0);
      }
      &:hover {
        transform: translate(0, 0);
        .sidebar-container {
          transform: translate(0, 0);
        }
      }
    }
    .main-container {
      margin-left: 40px;
    }
  }
  .sidebar-wrapper {
    width: 240px;
    position: fixed;
    top: 0;
    bottom: 0;
    left: 0;
    z-index: 1001;
    overflow: hidden;
    transition: all 0.28s ease-out;
  }
  .sidebar-container {
    transition: all 0.28s ease-out;
    position: absolute;
    top: 0;
    bottom: 0;
    left: 0;
    right: -17px;
    overflow-y: scroll;
  }
  .main-container {
    min-height: 100%;
    transition: all 0.28s ease-out;
    margin-left: 240px;
  }
}
.trademark {
  height: 70px;
  width: 100%;
}
.trademark .trademark-img {
  height: 100%;
  width: 12%;
  float: left;
}
.trademark .trademark-img img {
  height: 60%;
  width: auto;
  margin: 15px 20px 5px 20px;
}
.trademark .company {
  width: 58%;
  float: left;
  line-height: 70px;
}
.trademark .qq {
  float: right;
  margin-right: 50px;
  line-height: 70px;
}
</style>
