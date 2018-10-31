<template>
  <div>
    <el-tabs v-model="activeName" @tab-click="handleClick">
    <el-tab-pane label="微信付款流程" name="1">
      <ul>
       <li>
         <div class="title">第一步：打开微信，右上角有个加号，点击收付款</div>
         <div class="img_content">
           <img src="../../assets/img/wc_1.png" alt="">
         </div>
       </li>
       <li>
         <div class="title">第二步：点击查看付款码数字</div>
         <div class="img_content">
           <img src="../../assets/img/wc_2.png" alt="">
         </div>
       </li>
       <li>
         <div class="title">第三步：点击知道了</div>
         <div class="img_content">
           <img src="../../assets/img/wc_3.png" alt="">
         </div>
       </li>
       <li>
         <div class="title">第四步：查看付款码</div>
         <div class="img_content">
           <img src="../../assets/img/wc_4.png" alt="">
         </div>
       </li>
     </ul>
     </el-tab-pane>
    <el-tab-pane label="支付宝付款流程" name="2">
       <ul>
       <li>
         <div class="title">第一步：打开支付宝首页，点击付钱</div>
         <div class="img_content">
           <img src="../../assets/img/ali_1.png" alt="">
         </div>
       </li>
       <li>
         <div class="title">第二步：点击查看数字</div>
         <div class="img_content">
           <img src="../../assets/img/ali_2.png" alt="">
         </div>
       </li>
       <li>
         <div class="title">第三步：点击我知道了</div>
         <div class="img_content">
           <img src="../../assets/img/ali_3.png" alt="">
         </div>
       </li>
       <li>
         <div class="title">第四步：查看付款码</div>
         <div class="img_content">
           <img src="../../assets/img/ali_4.png" alt="">
         </div>
       </li>
     </ul>
    </el-tab-pane>
  </el-tabs>
     
  </div>
</template>
<script>
export default {
  data() {
    return {
      activeName: "1"
    };
  },
  methods: {
    handleClick(tab, event) {
      
    }
  }
};
</script>
<style scoped>
img {
  width: 100%;
  height: 100%;
}
ul li {
  list-style-type: none;
  width: 23%;
  margin-left: 2%;
  float: left;
}
.title {
  color: red;
  margin-bottom: 10px;
  font-size: 13px;
}
</style>
