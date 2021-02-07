from apscheduler.schedulers.background import BackgroundScheduler
from django_apscheduler.jobstores import DjangoJobStore
from django_apscheduler.models import DjangoJobExecution
import sys, environ

from pytz import utc


# This is the function you want to schedule - add as many as you want and then register them in the start() function below
def price_update():
    print("Scheduled function is on")


def start():
    scheduler = BackgroundScheduler(timezone=utc)
    scheduler.add_jobstore(DjangoJobStore(), "default")
    # run this job every PERIOD
    scheduler.add_job(price_update, 'interval', seconds=60, name='price update', jobstore='default',
                      id='price_update_job', replace_existing=True)
    #Now it's automatic
    #register_events(scheduler)
    scheduler.start()
    print("Scheduler started...")