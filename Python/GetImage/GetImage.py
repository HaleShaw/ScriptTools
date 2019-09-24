import requests
import os
from lxml import etree
from threading import *
from time import sleep

# Max number of thread
threadNum = 3
ThreadLock = BoundedSemaphore(threadNum)

globalHeader = {
    "User-Agent": "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/77.0.3865.90 Safari/537.36",
}

imgPath = "./images/"
resolution = "1920x1200"
mainUrl = "https://wallpaperscraft.com/all%s%s/page%d"
sortord = {"date": "/", "downloads": "/downloads/", "ratings": "/ratings/"}


class GetImage(Thread):
    def __init__(self, referer, url, title):
        Thread.__init__(self)
        self.referer = referer
        self.url = url.replace("300x188", resolution)
        if ("," in title):
            self.title = title.split(",")[0].replace("Preview wallpaper ", "")
        else:
            self.title = title

    def run(self):
        try:
            self.saveImage()
        finally:
            ThreadLock.release()

    def saveImage(self):
        currentHeader = {
            "Referer": self.referer
        }
        currentHeader.update(globalHeader)

        if not os.path.exists(imgPath):
            os.makedirs(imgPath)

        print("Downloading %s.jpg\n" % self.title)
        html = requests.get(self.url, headers=currentHeader)
        if html.status_code == 200:
            with open(imgPath + self.title + ".jpg", "wb") as f:
                f.write(html.content)
        elif html.status_code == 404:
            sleep(0.05)
        else:
            return None


def main():
    while True:
        try:
            pageNum = int(raw_input("Input how many pages to download:"))
            if pageNum > 0:
                break
        except ValueError:
            print("Please input a number!")
            continue
    for i in range(pageNum):
        url = mainUrl % (sortord.get("date"), resolution, (i + 1))
        html = requests.get(url, headers=globalHeader)
        if html.status_code == 200:
            xmlContent = etree.HTML(html.content)
            srcList = xmlContent.xpath(
                "//li[@class='wallpapers__item']//img/@src")
            titleList = xmlContent.xpath(
                "//li[@class='wallpapers__item']//img/@alt")
            for j in range(len(srcList)):
                ThreadLock.acquire()
                t = GetImage(url, srcList[j], titleList[j])
                t.start()


if __name__ == '__main__':
    main()
