from apscheduler.schedulers.background import BackgroundScheduler
from django_apscheduler.jobstores import DjangoJobStore
from djongo.sql2mongo import SQLDecodeError
from apl_api.ebay import ebay_update_observed_product_price
from pytz import utc
import environ

env = environ.Env()
base = environ.Path(__file__) - 2
environ.Env.read_env(env_file=base('.env'))
PERIOD = env.int('PERIODIC_UPDATE')

# This is the function you want to schedule - add as many as you want and then register them in the start() function below
def price_update():
    print("Scheduled function is on")
    ebay_update_observed_product_price()


def start():
    scheduler = BackgroundScheduler(timezone=utc)
    try:
        scheduler.add_jobstore(DjangoJobStore(), "default")
        # run this job every PERIOD
        scheduler.add_job(price_update, 'interval', replace_existing=True, seconds=PERIOD, name='price update',
                            jobstore='default', id='price_update_job')
        scheduler.start()
    except SQLDecodeError:
        scheduler.remove_job('price_update_job')
        scheduler.start()
    #Now it's automatic
    #register_events(scheduler)
    print("Scheduler started...")