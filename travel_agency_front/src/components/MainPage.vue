<template>
  <div class="p-4 p-md-5 mb-4 rounded text-body-emphasis bg-body-secondary">
    <div class="col-lg-6 px-0">
      <h1 class="display-4 fst-italic">Traveler</h1>
      <p class="lead my-3">Welcome to Traveler, your ultimate guide to discovering the world's most captivating
        destinations.
        Whether you're a seasoned globetrotter or embarking on your first adventure, we're here to inspire your
        journeys and make your travel dreams a reality. From hidden gems in bustling cities to serene retreats in
        nature's lap, we offer
        expert tips, curated itineraries, and vibrant stories that bring every corner of the globe to life. Join us as
        we explore the rich
        tapestry of cultures, flavors, and experiences that await you on your next trip. Your adventure begins here.</p>
    </div>
  </div>

  <div style="text-align: center;" v-if="user.role === 'ADMIN' || user.role === 'EDITOR'">
    <button class="button-create-action" @click="$router.push('/createTour')">Add tour</button>
  </div>

  <div style="text-align: center; margin-bottom: 30px;">
    <div class="form-control">
      <input class="input input-alt" placeholder="Type name of tour" type="text" v-model="inputTour"
        @input="searchTour">
      <span class="input-border input-border-alt"></span>
    </div>
    <div class="error-search" v-if="searchedTours.length === 0">Tours not found</div>
  </div>

  <div class="row mb-2" style="width: 100%;">
    <div class="col-md-6" v-for="(tour, index) in searchedTours" :key="index">
      <div style="cursor: pointer;"
        class="row g-0 border rounded overflow-hidden flex-md-row mb-4 shadow-sm h-md-250 position-relative"
        @click="enterTourPage(tour.id)">
        <div class="col p-4 d-flex flex-column position-static">
          <h4 style="max-width: 100%; word-wrap: break-word;" class="mb-0">{{ tour.name }}</h4>
          <div class="mb-1 text-body-secondary">{{ tour.startDate }} --- {{ tour.endDate }}</div>
          <p style="max-height: 145px; overflow: hidden;" class="card-text mb-auto">{{ tour.description }}</p>
        </div>
        <div class="col-auto d-none d-lg-block">
          <img class="bd-placeholder-img" :src="tour.imageUrl" alt="Description of image" width="390" height="250">
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import * as tourAPI from '@/services/API/tourAPI';
import * as userAPI from '@/services/API/userAPI';
import * as dateHelper from '@/js/dateHelper';

export default {
  data() {
    return {
      tours: [],
      searchedTours: [],
      inputTour: "",
      user: { role: "" }
    }
  },

  methods: {
    async enterTourPage(tourId) {
      this.$router.push("/tour/" + tourId);
    },

    async searchTour() {
      this.searchedTours = this.tours.filter(tour => tour.name.toLowerCase().includes(this.inputTour.toLowerCase()));
    }
  },

  async mounted() {
    this.tours = await tourAPI.getAvailableTours();
    if (!this.tours) {
      return;
    } else {
      for (let tour of this.tours) {
        tour.startDate = await dateHelper.formatDate(tour.startDate);
        tour.endDate = await dateHelper.formatDate(tour.endDate);
      }
      this.searchedTours = this.tours;
    }

    const token = localStorage.getItem('token');
    if (token) {
      this.user = await userAPI.getUserByToken(token);
      if (!this.user) this.user = { role: "" };
    }

  }
}

</script>

<style>
@import "./../assets/css/styleInputSearch.css";
@import "./../assets/css/styleButtonCreate.css";
</style>