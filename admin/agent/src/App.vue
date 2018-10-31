<template>
  <div id="app" :style="'height:'+fullHeight+'px'">
    <router-view></router-view>
  </div>
</template>

<script>
export default {
  name: 'app',
  data() {
    return {
      fullHeight: document.documentElement.clientHeight
    }
  }
}
</script>

<style lang="scss">
@import '~normalize.css/normalize.css'; // normalize.css 样式格式化
@import './styles/index.scss'; // 全局自定义的css样式
</style>
