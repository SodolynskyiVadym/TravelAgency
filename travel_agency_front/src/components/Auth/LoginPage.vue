<template>
    <div class="container-user-input">
        <form class="form" action="">
            <p class="title">Login</p>
            <input placeholder="Email" class="username input" type="text" v-model="email">
            <input placeholder="Password" class="password input" type="password" v-model="password">
            <button class="btn" type="submit" @click="login">Login</button>
        </form>
    </div>

</template>

<script>
export default {
    data() {
        return {
            email: '',
            password: ''
        }
    },
    methods: {
        login() {
            console.log(this.username, this.password);
        }
    }
}
</script>

<style>
@import "./../../assets/css/styleLoginSignup.css";
</style>