export class MemberService {
    constructor() {
        this.endpoint = "/api/member/";
    }
    createMemberSessionBooking(booking) {
        let url = this.endpoint + "createMemberSessionBooking";
        return new Promise((fulfill, reject) => {
            $.ajax({
                url: url,
                type: "POST",
                dataType: "json",
                contentType: "application/json",
                data: JSON.stringify(booking)
            }).then((response) => {
                fulfill(response);
            }).fail((jqXHR, textStatus, error) => {
                reject(error);
            });
        });
    }
    getMemberSessionsByDate(userId, date) {
        let url = this.endpoint + "getMemberSessionsByDate";
        url += "?userId=" + userId;
        url += "&date=" + date.toJSON();
        return new Promise((fulfill, reject) => {
            $.get(url).then((response) => {
                fulfill(response);
            }).fail((error) => {
                reject(error);
            });
        });
    }
    getMemberSessionsByDateRange(userId, startDate, endDate) {
        let url = this.endpoint + "getMemberSessionsByDateRange";
        url += "?userId=" + userId;
        url += "&startDate=" + startDate.toJSON();
        url += "&endDate=" + endDate.toJSON();
        return new Promise((fulfill, reject) => {
            $.get(url).then((response) => {
                fulfill(response);
            }).fail((error) => {
                reject(error);
            });
        });
    }
    getWorkoutAreaSessions(workoutAreaId, date) {
        let url = this.endpoint + "getWorkoutAreaSessions";
        url += "?workoutAreaId=" + workoutAreaId;
        url += "&date=" + date.toJSON();
        return new Promise((fulfill, reject) => {
            $.get(url).then((response) => {
                fulfill(response);
            }).fail((jqXHR, textStatus, error) => {
                reject(error);
            });
        });
    }
    getWorkoutAreas() {
        let url = this.endpoint + "getWorkoutAreas";
        return new Promise((fulfill, reject) => {
            $.get(url)
                .then((response) => {
                fulfill(response);
            })
                .fail((jqXHR, textStatus, error) => {
                reject(error);
            });
        });
    }
    isMemberSessionBookingValid(booking) {
        let url = this.endpoint + "isMemberSessionBookingValid";
        return new Promise((fulfill, reject) => {
            $.ajax({
                url: url,
                type: "POST",
                dataType: "json",
                contentType: "application/json",
                data: JSON.stringify(booking)
            }).then((response) => {
                fulfill(response);
            }).fail((jqXHR, textStatus, error) => {
                reject(error);
            });
        });
    }
}
//# sourceMappingURL=MemberService.js.map