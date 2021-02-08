from apscheduler.schedulers.background import BackgroundScheduler
from django_apscheduler.jobstores import DjangoJobStore

from apl_api.ebay import ebay_update_observed_product_price

from pytz import utc

# This is the function you want to schedule - add as many as you want and then register them in the start() function below
def price_update():
    print("Scheduled function is on")
    ebay_update_observed_product_price()


def start():
    scheduler = BackgroundScheduler(timezone=utc)
    scheduler.add_jobstore(DjangoJobStore(), "default")
    # run this job every PERIOD
    #scheduler.add_job(price_update, 'interval', replace_existing=True, seconds=120, name='price update',
     #                 jobstore='default', id='price_update_job')
    #Now it's automatic
    #register_events(scheduler)
    scheduler.start()
    print("Scheduler started...")