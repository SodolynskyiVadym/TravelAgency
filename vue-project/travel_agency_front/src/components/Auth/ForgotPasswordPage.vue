<template>
    <div class="container-user-input">
        <div class="form">
            <p class="title">Forgot password</p>
            <input placeholder="Email" class="username input" type="text" v-model="user.email" :readonly="isPasswordSended" @input="checkEmail">
            <input placeholder="Password" class="password input" type="password" v-if="isPasswordSended && isPasswordCreated" v-model="user.password">
            <button class="btn" type="submit" v-if="!isPasswordSended && isCorrectEmail" @click="createReservePassword">Send password</button>
            <button v-if="isPasswordSended && !isPasswordCreated" class="btn" type="button">
                <span class="spinner-border spinner-border-sm" aria-hidden="true"></span>
                <span role="status">Loading...</span>
            </button>
            <button class="btn" type="submit" v-if="isPasswordSended && user.password" @click="loginViaReservePassword">Login</button>
        </div>
    </div>
</template>

<script>
import * as userAPI from '@/services/API/userAPI';
import * as validEmail from '@/js/validEmail';


export default {
    data() {
        return {
            user: {
                email: '',
                password: ''
            },
            isCorrectEmail: false,
            isPasswordSended: false,
            isPasswordCreated: false
        }
    },
    methods: {
        async checkEmail() {
            this.isCorrectEmail = await validEmail.isValidEmail(this.user.email);
        },


        async createReservePassword() {
            this.isPasswordSended = true;
            await userAPI.createReservePassword(this.user.email);
            this.isPasswordCreated = true;
        },


        async loginViaReservePassword() {
            console.log(this.user);
            const data = await userAPI.loginViaReservePassword(this.user);
            console.log(data);
            if (data) {
                localStorage.setItem('token', data.token);
                this.$router.push('/');
                setTimeout(() => { window.location.reload(); }, 10);
            }
        }
    }
}
</script>

<style>
@import "./../../assets/css/styleLoginSignup.css";
</style>