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

  <div class="row mb-2" style="width: 100%;">
    <div class="col-md-6" v-for="(tour, index) in tours" :key="index">
      <div style="cursor: pointer;" class="row g-0 border rounded overflow-hidden flex-md-row mb-4 shadow-sm h-md-250 position-relative" 
        @click="enterTourPage(tour.id)">
        <div class="col p-4 d-flex flex-column position-static">
          <h4 style="max-width: 100%; word-wrap: break-word;" class="mb-0">{{ tour.name }}</h4>
          <div class="mb-1 text-body-secondary">{{ tour.startDate }} - {{ tour.endDate }}</div>
          <p class="card-text mb-auto">{{ tour.description }}</p>
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
export default {
  data() {
    return {
      tours: []
    }
  },

  methods: {
    async enterTourPage(tourId) {
      this.$router.push("/tour/" + tourId);
    }
  },

  async mounted() {
    this.tours = await tourAPI.getAllTours();
  }
}

</script>
