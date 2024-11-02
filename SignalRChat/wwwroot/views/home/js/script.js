import './gallery-init.js';
import './custom.js';


document.addEventListener("DOMContentLoaded", function () {
    var startDate = new Date("2024-07-01");
    var monthNames = ["Oca", "Şub", "Mar", "Nis", "May", "Haz", "Tem", "Ağu", "Eyl", "Eki", "Kas", "Ara"];
    var today = new Date();
    var startMonth = monthNames[startDate.getMonth()];
    var startYear = startDate.getFullYear();
    var yearDiff = today.getFullYear() - startDate.getFullYear();
    var monthDiff = today.getMonth() - startDate.getMonth();
    if (monthDiff < 0) {
        yearDiff--;
        monthDiff += 12;
    }
    var totalMonths = (yearDiff * 12 + monthDiff) + 1;
    var durationText = totalMonths + " ay";
    var workDateText = startMonth + " " + startYear + " - Halen · " + durationText;
    document.getElementById("jbm").innerHTML = workDateText;
});

document.addEventListener("DOMContentLoaded", function () {
    var internshipStartDate = new Date("2023-09-01"); // Eylül 2023
    var internshipEndDate = new Date("2024-06-01");   // Haziran 2024
    var monthNames = ["Oca", "Şub", "Mar", "Nis", "May", "Haz", "Tem", "Ağu", "Eyl", "Eki", "Kas", "Ara"];
    var startMonth = monthNames[internshipStartDate.getMonth()];
    var startYear = internshipStartDate.getFullYear();
    var endMonth = monthNames[internshipEndDate.getMonth()];
    var endYear = internshipEndDate.getFullYear();
    var yearDiff = internshipEndDate.getFullYear() - internshipStartDate.getFullYear();
    var monthDiff = internshipEndDate.getMonth() - internshipStartDate.getMonth();
    if (monthDiff < 0) {
        yearDiff--;
        monthDiff += 12;
    }
    var totalMonths = (yearDiff * 12 + monthDiff) + 1;
    var durationText = totalMonths + " ay";
    var internshipDateText = startMonth + " " + startYear + " - " + endMonth + " " + endYear + " · " + durationText;
    document.getElementById("netas").innerHTML = internshipDateText;
});

document.addEventListener("contextmenu", function (event) {
    event.preventDefault();
});