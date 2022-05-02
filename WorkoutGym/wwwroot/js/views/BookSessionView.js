import { MemberService } from "services/MemberService";
export class BookSessionView {
    constructor(service) {
        this.service = service;
        this.$container = $.dataJs("book-session");
        this.viewModel = null;
        this.init();
        this.registerEvents();
        this.loadWorkoutAreas();
        this.loadMemberSessions();
    }
    init() {
        let item = this.$container[0];
        this.$workoutAreas = this.$container.find("#workoutAreas");
        let $select = this.$container.find("#workoutSessions");
        this.$workoutSessions = $select.selectize();
        this.$sessionsContainer = this.$container.find("#sessionsContainer");
        this.$sessionsContainer.hide();
        this.$newSession = this.$container.find("#newSession");
        this.userId = this.$container.find("#userId").val();
        this.$sessionModal = this.$container.find("#newSessionModal");
        this.$validationMessage = this.$container.find("#validationMessage");
        this.$createSessionButton = this.$container.find("#createSessionButton");
        this.$memberSessionsFilter = this.$container.find("#memberSessionsFilter");
        this.dataTable = this.$container.find("#memberSessions").DataTable({
            pageLength: 10,
            columnDefs: [
                { targets: 0, visible: false }
            ],
            columns: [
                { data: "sessionId" },
                { data: "date" },
                { data: "area" },
                { data: "startTime" },
                { data: "endTime" }
            ]
        });
        this.$sessionDate = this.$container.find("#sessionDate").datepicker({
            startDate: $.currentDate(),
            endDate: $.endCurrentMonthDate(),
            todayHighlight: true,
            format: "yyyy-mm-dd"
        });
        this.$sessionDate.datepicker("setDate", $.currentDate());
        this.$showCalendarButton = this.$container.find("#showCalendarButton");
    }
    registerEvents() {
        this.$newSession.on("click", () => {
            this.$sessionModal.modal("show");
        });
        this.$workoutAreas.on("change", () => {
            this.clearValidationMessages();
            let selectize = this.$workoutSessions[0].selectize;
            selectize.clearOptions(true);
            if (this.$workoutAreas.val() !== "0") {
                this.loadWorkoutSessions();
            }
            else {
                this.$sessionsContainer.hide();
            }
        });
        this.$showCalendarButton.on("click", () => {
            this.$sessionDate.datepicker("show");
        });
        this.$workoutSessions.on("change", () => {
            this.onSessionChange();
        });
        this.$createSessionButton.on("click", () => {
            this.service.createMemberSessionBooking(this.viewModel)
                .then((data) => {
                this.clearValidationMessages();
                this.$validationMessage.addClass("text-info");
                this.$validationMessage.append(`Session, ${data.startTime} - ${data.endTime}, booked.`);
                this.$createSessionButton.prop("disabled", true);
                this.loadMemberSessions();
            }).catch((error) => {
                this.clearValidationMessages();
                this.$validationMessage.addClass("text-danger");
                this.$validationMessage.append(error);
            });
        });
        this.$memberSessionsFilter.on("change", () => {
            this.loadMemberSessions();
        });
        this.$sessionModal.on("hidden.bs.modal", () => {
            this.onModalClosed();
        });
    }
    loadWorkoutAreas() {
        this.service.getWorkoutAreas().then((data) => {
            this.$workoutAreas.append($("<option>", {
                value: 0,
                text: "Select Area"
            }));
            for (let i = 0; i < data.length; i++) {
                this.$workoutAreas.append($("<option>", {
                    value: data[i].id,
                    text: data[i].name
                }));
            }
        }).catch((error) => {
            console.log(error);
        });
    }
    loadWorkoutSessions() {
        let selectize = this.$workoutSessions[0].selectize;
        let workoutAreaId = this.$workoutAreas.val();
        let date = this.$sessionDate.datepicker("getDate");
        this.service.getWorkoutAreaSessions(workoutAreaId, date).then((data) => {
            selectize.addOption({
                value: 0,
                text: "Select Session"
            });
            for (let i = 0; i < data.length; i++) {
                let option = null;
                if (!data[i].enabled) {
                    option = {
                        value: data[i].sessionId,
                        text: data[i].display,
                        disabled: true
                    };
                }
                else {
                    option = {
                        value: data[i].sessionId,
                        text: data[i].display
                    };
                }
                selectize.addOption(option);
            }
            selectize.refreshOptions(false);
            selectize.setValue("0", true);
            this.$sessionsContainer.show();
        }).catch((error) => {
            console.log(error);
        });
    }
    loadMemberSessions() {
        let filter = this.$memberSessionsFilter.val();
        this.dataTable.clear();
        if (filter === "1") {
            this.service.getMemberSessionsByDate(this.userId, $.currentDate())
                .then((data) => {
                this.dataTable.rows.add(data);
                this.dataTable.draw();
            })
                .catch((error) => {
                console.log(error);
            });
        }
        else if (filter === "2") {
            this.service.getMemberSessionsByDateRange(this.userId, $.startCurrentMonthDate(), $.endCurrentMonthDate())
                .then((data) => {
                this.dataTable.rows.add(data);
                this.dataTable.draw();
            }).catch((error) => {
                console.log(error);
            });
        }
    }
    onSessionChange() {
        this.clearValidationMessages();
        let selectize = this.$workoutSessions[0].selectize;
        let workoutSessionId = parseInt(selectize.getValue());
        if (workoutSessionId > 0) {
            let workoutAreaId = parseInt(this.$workoutAreas.val());
            this.viewModel = {
                date: this.$sessionDate.datepicker("getDate"),
                workoutAreaId: workoutAreaId,
                workoutSessionId: workoutSessionId,
                userId: this.userId
            };
            this.service.isMemberSessionBookingValid(this.viewModel).then((data) => {
                if (data.isValid) {
                    this.$validationMessage.addClass("text-info");
                    this.$createSessionButton.prop("disabled", false);
                }
                else {
                    this.viewModel = null;
                    this.$validationMessage.addClass("text-danger");
                    this.$createSessionButton.prop("disabled", true);
                }
                this.$validationMessage.append(data.message);
            }).catch((error) => {
                console.log(error);
            });
        }
    }
    clearValidationMessages() {
        this.$validationMessage.removeClass("text-danger");
        this.$validationMessage.removeClass("text-danger");
        this.$validationMessage.empty();
    }
    onModalClosed() {
        this.$sessionDate.datepicker("setDate", $.currentDate());
        this.clearValidationMessages();
        let selectize = this.$workoutSessions[0].selectize;
        selectize.clearOptions(true);
        this.$sessionsContainer.hide();
        this.$workoutAreas.val("0");
    }
}
var bookSessionView = new BookSessionView(new MemberService());
//# sourceMappingURL=BookSessionView.js.map