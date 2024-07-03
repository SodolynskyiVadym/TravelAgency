<template>
    <div class="container-user-input">
        <div class="form">
            <p class="title">Sign Up</p>
            <input placeholder="Email" class="username input" type="text" v-model="user.email" @input="checkEmail">
            <input placeholder="Password" class="password input" type="password" v-model="user.password">
            <input placeholder="Confirm Password" class="password input" type="password" v-model="confirmPassword">

            <div v-if="message">{{ message }}</div>
            <button class="btn" v-if="isCorrectEmail && user.password.length >= 8 && user.password === confirmPassword"
                type="submit" @click="signUp">Sign Up</button>
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
                password: '',
            },
            confirmPassword: '',
            message: "",
            isCorrectEmail: false

        }
    },
    methods: {
        async checkEmail() {
            this.isCorrectEmail = await validEmail.isValidEmail(this.user.email);
        },


        async signUp() {
            this.message = "";
            const data = await userAPI.registerUser(this.user);
            if (data === false) {
                this.message = "You were not registered. Please try again."
            } else {
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