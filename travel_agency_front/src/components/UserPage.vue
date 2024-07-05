<template>
    <div class="container-user-input" v-if="isLoaded">
        <div class="form" style="height: 700px;">
            <p class="title">Your data</p>
            <label for="email">Email:</label>
            <input id="email" placeholder="Email" class="username input" type="text" v-model="user.email" readonly>

            <label for="role">Role:</label>
            <input id="role" placeholder="Role" class="username input" type="text" v-model="user.role" readonly>

            <label for="password">Password:</label>
            <input id="password" placeholder="Password" class="password input" type="password" v-model="password">

            <label for="confirmPassword">Confirm new password:</label>
            <input id="confirmPassword" placeholder="Confirm password" class="password input" type="password"
                v-model="confirmPassword">

            <p style="color: red; margin-top: 40px;" v-if="message">{{ message }}</p>
            <p v-if="password != confirmPassword || password.length < 8" style="color: wheat; margin-top: 40px;">New password must be more than 8</p>
            <button style="margin-top: 10px;" class="btn" type="submit" @click="updatePassword" v-else>Update password</button>
        </div>
    </div>

    <div style="margin-top: 40px;">
        <h2 style="text-align: center;">Your payments</h2>
        <table class="list-table">
            <tr>
                <th>Image</th>
                <th>Tour</th>
                <th>Start date</th>
                <th>End date</th>
                <th>Purchased seats</th>
                <th>Total price</th>
                <th>Status</th>
                <th>Action</th>
            </tr>
            <tr v-for="payment in userPayments" :key="payment.id">
                <td><img :src="payment.tour.imageUrl"></td>
                <td>{{ payment.tour.name }}</td>
                <td>{{ payment.tour.startDate }}</td>
                <td>{{ payment.tour.endDate }}</td>
                <td>{{ payment.amount }}</td>
                <td>{{ payment.amount * payment.tour.price }}</td>
                <td>{{ payment.isPaid ? 'Paid' : 'Unpaid' }}</td>
                <td>
                    <button v-if="!payment.isPaid" class="button-update-delete button-update-delete-hover-green" @click="payExistStripeSession(payment.stripeSession)">Pay</button>
                    <button v-else class="button-update-delete button-update-delete-hover-black" @click="$router.push(`tour/${payment.tourId}`)">View</button>
                </td>
            </tr>
        </table>
    </div>
</template>

<script>
import * as userAPI from '@/services/API/userAPI';
import * as payAPI from '@/services/API/payAPI';
import * as dateHelper from '@/js/dateHelper';

export default {
    data() {
        return {
            user: null,
            userPayments : [],
            password: "",
            confirmPassword: "",
            message: "",
            isLoaded: false
        }
    },
    methods: {
        async updatePassword() {
            this.message = "";
            const token = localStorage.getItem('token');
            if (token) {
                const result = await userAPI.updatePassword(this.password, token);
                if (result === false) {
                    this.message = "Password wasn't changed"
                }
                this.password = "";
                this.confirmPassword = "";
            } else this.$router.push('/login');
        },


        async payExistStripeSession(sessionStripeId){
            await payAPI.payExistingPayment(sessionStripeId);
        }
    },

    async mounted() {
        const token = localStorage.getItem('token');
        if (token) {
            this.user = await userAPI.getUserByToken(token);
            this.userPayments = await payAPI.getPaymentsByUser(token);
            for(let payment of this.userPayments){
                payment.tour.startDate = await dateHelper.formatDate(payment.tour.startDate);
                payment.tour.endDate = await dateHelper.formatDate(payment.tour.endDate);
            }
            this.isLoaded = true
        } else{
            localStorage.removeItem('token');
            this.$router.push('/login');
        }
    }
}
</script>

<style>
@import "./../assets/css/styleLoginSignup.css";
@import "./../assets/css/styleTable.css";
@import "./../assets/css/styleButtonCreate.css";
</style>