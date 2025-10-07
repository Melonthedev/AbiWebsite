window.startCountdown = function (elementId, targetDateString) {
    const targetDate = new Date(targetDateString);
    const countdownEl = document.getElementById(elementId);

    function pad(n) {
        return n.toString().padStart(2, '0');
    }

    function updateCountdown() {
        const now = new Date();
        let diff = targetDate - now;

        if (diff <= 0) {
            countdownEl.innerHTML = '<div class="countdown-block">🎉 ABI TIME!</div>';
            clearInterval(timer);
            return;
        }

        const months = Math.floor(diff / (1000 * 60 * 60 * 24 * 30));
        diff -= months * 1000 * 60 * 60 * 24 * 30;

        const days = Math.floor(diff / (1000 * 60 * 60 * 24));
        diff -= days * 1000 * 60 * 60 * 24;

        const hours = Math.floor(diff / (1000 * 60 * 60));
        diff -= hours * 1000 * 60 * 60;

        const minutes = Math.floor(diff / (1000 * 60));
        diff -= minutes * 1000 * 60;

        const seconds = Math.floor(diff / 1000);

        countdownEl.innerHTML = `
            <div class="countdown-block"><span>${months}</span><div class="countdown-label">Monate</div></div>
            <div class="countdown-block"><span>${days}</span><div class="countdown-label">Tage</div></div>
            <div class="countdown-block"><span>${pad(hours)}</span><div class="countdown-label">Stunden</div></div>
            <div class="countdown-block"><span>${pad(minutes)}</span><div class="countdown-label">Minuten</div></div>
            <div class="countdown-block"><span>${pad(seconds)}</span><div class="countdown-label">Sekunden</div></div>
        `;
    }

    updateCountdown();
    const timer = setInterval(updateCountdown, 1000);
};
