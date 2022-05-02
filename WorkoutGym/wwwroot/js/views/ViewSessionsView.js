import { AdminService } from "services/AdminService";
export class ViewSessionsView {
    constructor(service) {
        this.service = service;
        this.$container = $.dataJs("view-sessions");
        this.init();
        this.registerEvents();
        this.loadMemberSessions();
    }
    init() {
        this.$memberSessionsFilter = this.$container.find("#memberSessionsFilter");
        this.$downloadTimeTable = this.$container.find("#downloadTimeTable");
        this.$downloadTimeTable.hide();
        this.$loader = $(this.$container[0]).waitMe({
            effect: "bounce",
            bg: "rgb(255,255,255,0.7)"
        });
        this.dataTable = this.$container.find("#memberSessions").DataTable({
            pageLength: 10,
            columnDefs: [
                { targets: 0, visible: false }
            ],
            columns: [
                { data: "sessionId" },
                { data: "date" },
                { data: "member" },
                { data: "email" },
                { data: "area" },
                { data: "startTime" },
                { data: "endTime" }
            ]
        });
    }
    registerEvents() {
        this.$memberSessionsFilter.on("change", () => {
            this.loadMemberSessions();
        });
    }
    loadMemberSessions() {
        let filter = this.$memberSessionsFilter.val();
        this.dataTable.clear();
        this.$loader.waitMe("show");
        if (filter === "1") {
            this.$downloadTimeTable.show();
            this.service.getMemberSessionsByDate($.currentDate())
                .then((data) => {
                this.dataTable.rows.add(data);
                this.dataTable.draw();
                this.$loader.waitMe("hide");
            })
                .catch((error) => {
                console.log(error);
            });
        }
        else if (filter === "2") {
            this.$downloadTimeTable.hide();
            this.service.getMemberSessionsByDateRange($.startCurrentMonthDate(), $.endCurrentMonthDate())
                .then((data) => {
                this.dataTable.rows.add(data);
                this.dataTable.draw();
                this.$loader.waitMe("hide");
            }).catch((error) => {
                console.log(error);
            });
        }
    }
}
var viewSessionsView = new ViewSessionsView(new AdminService());
//# sourceMappingURL=ViewSessionsView.js.map