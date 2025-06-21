document.addEventListener('DOMContentLoaded', () => {
    const sliders = document.querySelectorAll('.slider-wrapper');

    sliders.forEach(slider => {
        const container = slider.closest('.position-relative');
        const btnLeft = container.querySelector('.btnLeft');
        const btnRight = container.querySelector('.btnRight');
        const scrollAmount = 300;

        btnRight?.addEventListener('click', () => {
            slider.scrollBy({ left: scrollAmount, behavior: 'smooth' });
        });

        btnLeft?.addEventListener('click', () => {
            slider.scrollBy({ left: -scrollAmount, behavior: 'smooth' });
        });
    });
});