<template>
    <div v-if="isLoaded" class="main">
        <h1>{{ tour.name }}</h1>
        <p class="tourDescription">{{ tour.description }}</p>
        <h5>{{ tour.startDate }} --- {{ tour.endDate }}</h5>
        <img :src="tour.imageUrl" alt="tour image" class="tourImage">


        <div>
            <div v-for="(destination, index) in tour.destinations" :key="index" class="location">
                <h2>{{ index + 1 }} location</h2>
                <h3>{{ destination.hotel.place.name }}({{ destination.hotel.place.country }})</h3>
                <h4>{{ destination.startDate }} --- {{ destination.endDate }}</h4>
                <p>{{ destination.hotel.place.description }}</p>
                <div class="locationImages">
                    <div v-for="imageUrl in destination.hotel.place.imagesUrls" :key="imageUrl.id">
                        <img style="width: 500px" :src="imageUrl.url" alt="location image">
                    </div>
                </div>
                <div class="hotel">
                    <h3>Your hotel({{ destination.hotel.place.name }})</h3>
                    <h4>"{{ destination.hotel.name }}"</h4>
                    <h4>Address: {{ destination.hotel.address }}</h4>
                    <p style="margin-top: 30px;">{{ destination.hotel.description }}</p>
                    <img :src="destination.hotel.imageUrl" alt="hotel image" class="hotelImage">
                </div>
            </div>
        </div>



        <div v-if="user.role">
            <div style="margin-top: 60px; text-align: center;">
                <h2>Quantity</h2>
                <input type="number" v-model="quantity" min="1" max="10" @input="calculatePrice">
            </div>
            <div>
                <h2 style="margin-top: 30px;">Price: {{ tour.price }}</h2>
            </div>

            <div style="margin-top: 30px; display: flex; flex-direction: column;">
                <button class="button-action" @click="reserveTour" v-if="!isSendRequest && !haveUserTour"
                    :disabled="isSendRequest">Buy</button>
                <button class="button-action" @click="enterUpdatePage"
                    v-if="user.role === 'EDITOR' || user.role === 'ADMIN'">Update</button>
            </div>
        </div>

    </div>



    <div style="text-align: center; margin-top: 70px;">
        <h2>Your review</h2>
        <div v-if="user.role" style="text-align:center; ">
            <div>
                <div class="rating">
                    <input value="5" name="rating" id="star5" type="radio">
                    <label for="star5"></label>
                    <input value="4" name="rating" id="star4" type="radio">
                    <label for="star4"></label>
                    <input value="3" name="rating" id="star3" type="radio">
                    <label for="star3"></label>
                    <input value="2" name="rating" id="star2" type="radio">
                    <label for="star2"></label>
                    <input value="1" name="rating" id="star1" type="radio">
                    <label for="star1"></label>
                </div>
            </div>
            <textarea class="comment-area" v-model="userReview.text" placeholder="Write your review"></textarea><br>
            <button v-if="!isReviewFromDb" class="button-comment-area" @click="sendReview">Send</button>
            <div v-else>
                <button class="button-comment-area" @click="updateReview">Update</button>
                <button class="button-comment-area" @click="deleteReview">Delete</button>
            </div>
        </div>
    </div>

    <div style="text-align: center; margin-top: 40px">
        <div v-for="review in reviews" :key="review.id" class="review">
            <h3>{{ review.rating }}</h3>
            <p>{{ review.text }}</p>
        </div>
    </div>
</template>

<script>
import * as tourAPI from '@/services/API/tourAPI';
import * as payAPI from '@/services/API/payAPI';
import * as userAPI from '@/services/API/userAPI';
import * as reviewAPI from '@/services/API/reviewAPI';
import * as dateHelper from '@/js/dateHelper';

export default {
    data() {
        return {
            user: { role: "" },
            userReview: { rating: 0, text: "" },
            reviews: [],
            tour: null,
            quantity: 1,
            priceForOne: 0,
            isSendRequest: false,
            isLoaded: false,
            isReviewFromDb: false,
            haveUserTour: true
        }
    },
    methods: {
        enterUpdatePage() {
            this.$router.push(`/updateTour/${this.tour.id}`);
        },

        async reserveTour() {
            this.isSendRequest = true;
            const data = {
                tourId: this.tour.id,
                quantity: this.quantity
            }
            const token = localStorage.getItem('token');
            if (!token) {
                this.$router.push('/login');
            }

            await payAPI.reserveTour(data, token);
        },


        async calculatePrice() {
            if (this.quantity < 1) {
                this.quantity = 1;
            }
            this.tour.price = this.priceForOne * this.quantity;
        },


        async sendReview(){
            const token = localStorage.getItem('token');
            if (!token) {
                this.$router.push('/login');
            }
            this.userReview.rating = document.querySelector('input[name="rating"]:checked').value;
            this.userReview.tourId = this.tour.id;

            await reviewAPI.createReview(this.userReview, token);
            this.isReviewFromDb = true;
            this.reviews = await reviewAPI.getTourReviews(this.tour.id);
        },


        async updateReview(){
            const token = localStorage.getItem('token');
            if (!token) {
                this.$router.push('/login');
            }
            this.userReview.rating = document.querySelector('input[name="rating"]:checked').value;
            this.userReview.tourId = this.tour.id;

            await reviewAPI.updateReview(this.userReview, token);
            this.reviews = await reviewAPI.getTourReviews(this.tour.id);
        },


        async deleteReview(){
            const token = localStorage.getItem('token');
            if (!token) {
                this.$router.push('/login');
            }

            await reviewAPI.deleteReview(this.$route.params.id, token);
            this.reviews = await reviewAPI.getTourReviews(this.tour.id);
            this.userReview = { rating: 0, text: "" };
            this.isReviewFromDb = false;
        }
    },

    async mounted() {
        this.tour = await tourAPI.getTourById(this.$route.params.id);
        this.priceForOne = this.tour.price;
        this.tour.startDate = await dateHelper.formatDate(this.tour.startDate);
        this.tour.endDate = await dateHelper.formatDate(this.tour.endDate);
        for (let destination of this.tour.destinations) {
            destination.startDate = await dateHelper.formatDate(destination.startDate);
            destination.endDate = await dateHelper.formatDate(destination.endDate);
        }

        const token = localStorage.getItem('token');
        if (token) {
            this.user = await userAPI.getUserByToken(token);
            if (!this.user) this.user = { role: "" };
        }

        this.isLoaded = true;

        if (this.user.role){
            const userReviewFromDb = await reviewAPI.getUserReview(this.tour.id, token);
            this.haveUserTour = await payAPI.haveUserPayment(this.tour.id, token);
            if (userReviewFromDb){
                this.userReview = userReviewFromDb;
                this.isReviewFromDb = true;
                document.querySelector(`#star${userReviewFromDb.rating}`).checked = true;
            }
        }

        this.reviews = await reviewAPI.getTourReviews(this.tour.id);
    }
}
</script>

<style>
@import "./../assets/css/styleTourPage.css";
@import "./../assets/css/styleReview.css";
</style>