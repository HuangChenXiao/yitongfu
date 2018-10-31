<template>
    <div v-show="visible">
        <div class="mask">
            <div class="mask-content">
                <div class="mask-item" v-for="(item,index) in explain_list" :key="item.id" v-if="index==curr_index">
                    <div class="m">
                        <img :src="item.flow_img" alt="暂无图片">
                    </div>
                    <div class="m-tool">
                        <div class="explain">
                            <div class="pc-e" v-if="index==0">如果您可以直接通过手机扫码请忽略以下流程</div>
                            {{item.flow_explain}}
                        </div>
                        <div class="explain-web">
                            {{item.flow_explain}}
                        </div>
                        <div class="opt-btn" @click="change_index()">
                            {{item.flow_btn}}
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
export default {
  props: ['value'],
  data() {
    return {
      curr_index: 0,
      visible: false,
      explain_list: [
        // {
        //   id: 1,
        //   flow_img: '/static/img/liu/1.jpg',
        //   flow_explain: '长按二维码',
        //   flow_btn: '下一步'
        // },
        {
          id: 2,
          flow_img: '/static/img/liu/2.jpg',
          flow_explain: '长按二维码，点击分享图片',
          flow_btn: '下一步'
        },
        {
          id: 3,
          flow_img: '/static/img/liu/3.jpg',
          flow_explain: '选择微信好友',
          flow_btn: '下一步'
        },
        {
          id: 4,
          flow_img: '/static/img/liu/4.jpg',
          flow_explain: '随便选择一个好友分享',
          flow_btn: '下一步'
        },
        {
          id: 5,
          flow_img: '/static/img/liu/5.jpg',
          flow_explain: '选择分享图片',
          flow_btn: '下一步'
        },
        {
          id: 6,
          flow_img: '/static/img/liu/6.jpg',
          flow_explain: '点击留在微信',
          flow_btn: '下一步'
        },
        {
          id: 7,
          flow_img: '/static/img/liu/7.jpg',
          flow_explain: '点击查看刚才分享的二维码图片',
          flow_btn: '下一步'
        },
        {
          id: 8,
          flow_img: '/static/img/liu/8.jpg',
          flow_explain: '长按二维码，点击识别图中二维码',
          flow_btn: '下一步'
        },
        {
          id: 9,
          flow_img: '/static/img/liu/9.jpg',
          flow_explain: '最后完成付款操作',
          flow_btn: '完成'
        }
      ]
    }
  },
  mounted() {
    if (this.value) {
      this.visible = true
    }
  },
  methods: {
    change_index() {
      this.curr_index++
      if (this.curr_index >= 8) {
        this.curr_index = 0
        this.visible = false
      }
    }
  },
  watch: {
    value(val) {
      console.log(val)

      this.visible = val
    },
    visible(val) {
      this.$emit('input', val)
    }
  }
}
</script>

<style scoped>
.mask {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background: rgba(0, 0, 0, 0.6);
  z-index: 9999;
}
.mask-content {
  position: absolute;
  top: 10%;
  left: 20%;
  width: 60%;
  height: 80%;
  /* background: #fff; */
  /* text-align: center; */
}
.mask-content .mask-item {
  position: relative;
  height: 100%;
}
.mask-content .m {
  height: 100%;
}
.mask-content .m img {
  width: 350px;
  height: 650px;
}
.mask-content .m-tool {
  position: absolute;
  left: 380px;
  bottom: 100px;
}
.mask-content .m-tool .explain {
  height: 108px;
  width: 407px;
  background: url('/static/img/web.png') no-repeat;
  background-size: 407px 108px;
  padding: 40px 30px 30px 50px;
  color: #fff;
}
.mask-content .m-tool .opt-btn {
  margin: 50px 0 0 50px;
  background: #f00;
  color: #fff;
  border-radius: 5px;
  width: 150px;
  height: 40px;
  line-height: 40px;
  text-align: center;
  cursor: pointer;
}
.mask-content .m-tool .opt-btn:active,
.mask-content .m-tool .opt-btn:hover {
  background: #ff6262;
}
.pc-e {
  color: #fff;
  font-weight: 600;
  margin-bottom: 5px;
}
.explain-web {
  display: none;
}
@media screen and (max-width: 640px) {
  .pc-e {
    display: none;
  }
  .explain-web {
    display: block;
  }
  .mask-content {
    position: absolute;
    top: 20px;
    left: 0;
    width: 100%;
    height: 70%;
    /* background: #fff; */
    text-align: center;
  }
  .mask-content .m img {
    width: 70%;
    height: 100%;
  }
  .mask-content .m-tool {
    position: relative;
    left: 0;
    bottom: 0;
  }
  .mask-content .m-tool .explain-web {
    color: #fff;
    margin: 30px auto;
    border: 1px dashed #fff;
    padding: 20px 0;
    width: 70%;
  }
  .mask-content .m-tool .explain {
    display: none;
  }
  .mask-content .m-tool .opt-btn {
    margin: 0 auto;
    background: #f00;
    color: #fff;
    border-radius: 5px;
    width: 150px;
    height: 40px;
    line-height: 40px;
    text-align: center;
    cursor: pointer;
  }
}
</style>