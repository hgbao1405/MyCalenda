var a = angular.module('calendarApp', []);
a.factory('EventFactory', function ($http) {
    return {
        GetList: function (curentday, callback) {
            const month = curentday.getMonth()+1;
            const year = curentday.getFullYear();
            $http.get(`/api/Events?month=${month}&year=${year}`).then(callback)
        }
    }
});
a.controller('CalendarController', function ($scope, EventFactory) {
    const monthNames = ["Tháng 1", "Tháng 2", "Tháng 3", "Tháng 4", "Tháng 5", "Tháng 6", "Tháng 7", "Tháng 8", "Tháng 9", "Tháng 10", "Tháng 11", "Tháng 12"];
    const today = new Date();
    
    $scope.currentDate = new Date();
    $scope.calendarDays = [];
    $scope.notes = {};
    $scope.currentDayNotes = [];
    $scope.form = {
        title: '',
        Color: '#ff0000'
    };
    $scope.selectedDay = null;
    $scope.today = `${today.getFullYear()}-${today.getMonth()}-${today.getDate()}`;

    $scope.init = function () {
        $scope.updateCalendar($scope.currentDate);
        $scope.GetListNote();
    }
    $scope.GetListNote = function () {
        $scope.notes = {};
        EventFactory.GetList($scope.currentDate,function (rs) {
            rs = rs.data;
            rs.forEach((item) => {
                var note = {
                    title: item.title,
                    Color: item.color
                }
                $scope.addNote(note, item.startTime);
            });
        })
    }
    $(function () {
        $('#datepicker').datepicker({
            changeMonth: true,
            changeYear: true,
            monthNamesShort: monthNames,
            showButtonPanel: true,
            onClose: function (dateText, inst) {
                const dt = new Date(inst.selectedYear, inst.selectedMonth, 1)
                $(this).datepicker('setDate', dt);
                $scope.$apply(() => {
                    $scope.ChooseMonthYear(dt);
                    $scope.updateCalendar($scope.currentDate);
                });
            }
        });
        $('#choosedate').click(function () {
            $('#datepicker').focus();
        })
    });

    $scope.ChooseMonthYear = function (dt) {
        $scope.currentDate.setMonth(dt.getMonth());
        $scope.currentDate.setFullYear(dt.getFullYear());
        $scope.GetListNote();
    }

    $scope.updateCalendar = function (currentMonthYear) {
        const year = currentMonthYear.getFullYear();
        const month = currentMonthYear.getMonth();
        const firstDay = new Date(year, month, 1).getDay();
        const daysInMonth = new Date(year, month + 1, 0).getDate();

        $scope.calendarDays = [];

        // Add empty days for alignment
        for (let i = 0; i < firstDay; i++) {
            $scope.calendarDays.push({ date: '' });
        }

        // Add calendar days
        for (let i = 1; i <= daysInMonth; i++) {
            const dayKey = `${year}-${month}-${i}`;
            $scope.calendarDays.push({
                date: i,
                isCurrentDay: dayKey == $scope.today,
                notes: $scope.notes[dayKey] || []
            });
        }

        $scope.monthYear = `${monthNames[month]} ${year}`;
        $scope.updateCurrentDayNotes();
    };

    $scope.previousMonth = function () {
        $scope.currentDate.setMonth($scope.currentDate.getMonth() - 1);
        $scope.updateCalendar($scope.currentDate);
        $scope.GetListNote();
    };

    $scope.nextMonth = function () {
        $scope.currentDate.setMonth($scope.currentDate.getMonth() + 1);
        $scope.updateCalendar($scope.currentDate);
        $scope.GetListNote();
    };

    $scope.selectDay = function (day) {
        if (day) {
            $scope.selectedDay = day;
            $scope.updateCurrentDayNotes();
        }
    };

    $scope.updateCurrentDayNotes = function () {
        if ($scope.selectedDay) {
            const year = $scope.currentDate.getFullYear();
            const month = $scope.currentDate.getMonth();
            const key = `${year}-${month}-${$scope.selectedDay}`;
            $scope.currentDayNotes = $scope.notes[key] || [];
        } else {
            $scope.currentDayNotes = [];
        }
    };

    $scope.addNote = function (note, selectedDay) {
        if (note.title.trim() !== '' && selectedDay) {
            const selectday = new Date(selectedDay);
            const year = selectday.getFullYear();
            const month = selectday.getMonth();
            const key = `${year}-${month}-${selectday.getDate()}`;

            if (!$scope.notes[key]) {
                $scope.notes[key] = [];
            }

            $scope.notes[key].push({
                text: note.title,
                color: note.Color
            });
            note.title = '';
            $scope.updateCalendar($scope.currentDate);
            $scope.updateCurrentDayNotes();
        }
    };

    $scope.selectCalendaDate = function () {
        if ($scope.selectedDay == null) {
            console.log("Bạn chưa chọn ngày")
            return "";
        }
        const year = $scope.currentDate.getFullYear();
        const month = $scope.currentDate.getMonth()+1;
        return `${year}-${month}-${$scope.selectedDay}`;
    }

    $scope.init();
});