<template>
    <div class="container-user-input">
        <form class="form" action="">
            <p class="title">Sign Up</p>
            <input placeholder="Email" class="username input" type="text" v-model="email">
            <input placeholder="Password" class="password input" type="password" v-model="password">
            <input placeholder="Confirm Password" class="password input" type="password" v-model="confirmPassword">
            <button class="btn" type="submit" @click="signUp">Sign Up</button>
        </form>
    </div>

</template>

<script>
export default {
    data() {
        return {
            email: '',
            password: '',
            confirmPassword: ''
        }
    },
    methods: {
        signUp() {
            console.log(this.username, this.password);
        }
    }
}
</script>

<style>
@import "./../../assets/css/styleLoginSignup.css";
</style>