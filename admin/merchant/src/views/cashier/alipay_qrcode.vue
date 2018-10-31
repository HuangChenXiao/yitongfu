<template>
    <div id="alipay">
        
    </div>
</template>

<script>
    export default {
        data(){
            return{
                str_data: JSON.parse(decodeURIComponent(this.$route.params.str_data))
            }
        },
        mounted(){
            console.log(this.str_data)
            // $("body").append(this.str_data)
        }
    }
</script>

<style scoped>

</style>