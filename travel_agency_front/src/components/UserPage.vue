<template>
    <div class="container-user-input" v-if="isLoaded">
        <div class="form" style="height: 700px;">
            <p class="title">Your data</p>
            <label for="email">Email:</label>
            <input id="email" placeholder="Email" class="username input" type="text" v-model="user.email" readonly>

            <label for="role">Role:</label>
            <input id="role" placeholder="Role" class="username input" type="text" v-model="user.role" readonly>

            <label for="password">Password:</label>
            <input id="password" placeholder="Password" class="password input" type="password" v-model="user.password">

            <label for="confirmPassword">Confirm Password:</label>
            <input id="confirmPassword" placeholder="Confirm password" class="password input" type="password"
                v-model="confirmPassword">

            <p style="color: wheat; margin-top: 40px;">New password must be more than 8</p>
            <button style="margin-top: 10px;" class="btn" type="submit" @click="updatePassword"
                :disabled="user.password != confirmPassword || user.password < 8">Update password</button>
        </div>
    </div>
</template>

<script>
import * as userAPI from '@/services/API/userAPI';

export default {
    data() {
        return {
            user: null,
            confirmPassword: "",
            isLoaded: false
        }
    },
    methods: {
        async updatePassword(){
            const token = localStorage.getItem('token');
            if (token) {
                await userAPI.updatePassword(this.user.password, token);
                this.user.password = "";
                this.confirmPassword = "";
            } else this.$router.push('/error');
        }
    },

    async mounted() {
        const token = localStorage.getItem('token');
        if (token) {
            this.user = await userAPI.getUserByToken(token);
            this.isLoaded = true
        } else this.$router.push('/error');
    }
}
</script>

<style>
@import "./../assets/css/styleLoginSignup.css";
</style>